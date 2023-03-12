using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Entities;
using Mapster;
using EntityFramework.Exceptions.Common;
using TimejApi.Helpers;
using TimejApi.Services.User;
using UserRoles = TimejApi.Data.Entities.User.Role;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserEntityController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserEntityController> _logger;

        public UserEntityController(IUserService userService, ILogger<UserEntityController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Public method to get some User data
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPublicDto>> Get(Guid id)
        {
            if (await _userService.DbContext.Users.FindAsync(id) is not User user)
                return NotFound();
            return Ok(user.Adapt<UserPublicDto>());
        }

        /// <summary>
        /// Method to get data of current authorized user 
        /// </summary>
        /// <remarks>
        /// Acceptable for any authorized user 
        /// </remarks>
        [HttpGet("details")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetDetails()
        {
            if (!User.TryGetIdentifierAsGuid(out var userId)) return BadRequest();

            if (await _userService.TryGet(userId) is not User user)
                return NotFound();
            return Ok(user.Adapt<UserDto>());
        }

        /// <summary>
        /// Protected method to get User data
        /// </summary>
        /// <remarks>
        /// Acceptable for owners and MODERATORs
        /// </remarks>
        [HttpGet("details/{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetDetails(Guid id)
        {
            if (!User.IdentifierEqualsOrInRole(id, nameof(UserRoles.MODERATOR))) return Forbid();

            if (await _userService.TryGet(id) is not User user)
                return NotFound();

            return Ok(user.Adapt<UserDto>());
        }

        /// <summary>
        /// Edits some partional data;
        /// </summary>
        /// <remarks>
        /// Acceptable for owners and MODERATORs
        /// </remarks>
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Patch(Guid id, UserEditDto data)
        {
            if (!User.TryGetIdentifierAsGuid(out var callerId) || callerId != id
                && !User.IsInRole(nameof(UserRoles.MODERATOR))) return Forbid();
            try
            {
                if (!(await _userService.TryEdit(id, data)).Ensure(out var user, out var problem))
                    return problem.ToActionResult();

                _logger.LogInformation("User ({}) edited user ({})", callerId, user.Id);

                return Ok(user.Adapt<UserDto>());
            }
            catch (UniqueConstraintException exception)
            {
                _logger.LogInformation(exception, "Occured while user ({}), tried to edit user ({})", callerId, id);
                return Problem(statusCode: StatusCodes.Status409Conflict,
                               title: "Cannot edit user",
                               detail: "Seemed like some identifiers already in use");
            }
        }

        /// <summary>
        /// Get filtered collection of users
        /// </summary>
        /// <param name="groupId">Filter by group</param>
        /// <param name="email">Get concrete user by email</param>
        /// <param name="role">Filter by role</param>
        /// <response code="400">When no filters were specified</response>
        /// <response code="403">
        /// Unauthorised user can only query teachers. 
        /// In other cases caller should has MODERATOR or SCHEDULE_EDITOR role.
        /// </response>
        [HttpGet]
        public async Task<ActionResult<UserDto[]>> QueryUsers(
            [FromQuery] Guid? groupId,
            [FromQuery][EmailAddress] string? email,
            [FromQuery] UserRoles? role)
        {
            if (groupId == null && email == null && role == null) 
                return Problem(statusCode: StatusCodes.Status400BadRequest,
                               title: "Invalid query",
                               detail: "No filtering variables were specified");

            if (!User.IsInRole(nameof(UserRoles.MODERATOR))
                && !User.IsInRole(nameof(UserRoles.SCHEDULE_EDITOR))
                && (role == null 
                    || role != UserRoles.TEACHER))
            {
                return Problem(statusCode: StatusCodes.Status403Forbidden,
                               title: "Forbidden",
                               detail: "Caller can't execute that query");
            }

            var users = await _userService
                .QueryUsers(groupId, email, role);

            return users.Adapt<UserDto[]>();
        }
    }
}
