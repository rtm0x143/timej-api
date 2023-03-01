using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Entities;
using Mapster;
using EntityFramework.Exceptions.Common;
using TimejApi.Helpers;
using TimejApi.Services.User;
using UserEntity = TimejApi.Data.Entities.User;
using UserRole = TimejApi.Data.Entities.User.Role;
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
            if (!User.IdentifierEqualsOrInRole(id, nameof(UserRole.MODERATOR))) return Forbid();

            if (await _userService.TryGet(id) is not User user)
                return NotFound();

            return Ok(user.Adapt<UserDto>());
        }

        /// <summary>
        /// Edits user entity
        /// </summary>
        /// <remarks>
        /// Acceptable for MODERATORs
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public async Task<ActionResult<UserDto>> Put(Guid id, UserRegister data)
        {
            User.TryGetIdentifierAsGuid(out var moderatorId);
            try
            {
                if (!(await _userService.TryChangeUser(id, data)).Ensure(out var user, out var problem))
                    return problem.ToActionResult();

                _logger.LogInformation("User ({}) has beed updated by Moderator ({})", user.Id, moderatorId);
                return user.Adapt<UserDto>();
            }
            catch (UniqueConstraintException exception)
            {
                _logger.LogWarning(exception, "Problem while Moderator ({}) tried to edit user ({})", moderatorId, id);
                return Problem(statusCode: StatusCodes.Status409Conflict,
                               title: "Cannot create user",
                               detail: "Seemed like some identifiers already in use");
            }
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
                && !User.IsInRole(nameof(UserRole.MODERATOR))) return Forbid();
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
        /// An method to register new user
        /// </summary>
        /// <remarks>
        /// Acceptable for MODERATORs
        /// </remarks>
        [HttpPost("register")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public async Task<ActionResult<UserDto>> RegisterUser(UserRegister userRegister)
        {
            User.TryGetIdentifierAsGuid(out var moderatorId);

            try
            {
                var newUser = await _userService.Register(userRegister);
                _logger.LogInformation("New User ({}) with roles [{}] has been created by Moderator ({}).",
                    newUser.Id, string.Join(", ", newUser.Roles), moderatorId);

                return Ok(newUser.Adapt<UserDto>());
            }
            catch (UniqueConstraintException ex)
            {
                _logger.LogTrace(ex, "While tring to create new User; by moderator ({})", moderatorId);
                return Problem(statusCode: StatusCodes.Status409Conflict,
                               title: "Cannot create user",
                               detail: "Seemed like some identifiers already in use");
            }
            catch (ReferenceConstraintException ex)
            {
                _logger.LogTrace(ex, "While tring to create new User; by moderator ({})", moderatorId);
                return Problem(statusCode: StatusCodes.Status400BadRequest,
                               title: "Cannot create user",
                               detail: "Seemed like you tring to reference unknown Group");
            }
        }

        /// <summary>
        /// Get filtered collection of users
        /// </summary>
        /// <param name="groupId">Filter by group</param>
        /// <param name="email">Get concrete user by email</param>
        /// <param name="teacher">Filter by by teacher role</param>
        /// <response code="404">When no filters were specified</response>
        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.SCHEDULE_EDITOR)},{nameof(UserRole.MODERATOR)}")]
        public async Task<ActionResult<UserDto[]>> QueryUsers(
            [FromQuery] Guid? groupId,
            [FromQuery][EmailAddress] string? email,
            [FromQuery] bool teacher = false)
        {
            if (groupId == null && email == null && !teacher) 
                return Problem(statusCode: StatusCodes.Status400BadRequest,
                               title: "Invalid query",
                               detail: "No filtering variables were specified");

            var users = await _userService
                .QueryUsers(groupId, email, teacher ? UserRole.TEACHER : null);

            return users.Adapt<UserDto[]>();
        }
    }
}
