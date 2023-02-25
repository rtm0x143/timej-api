using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Entities;
using Mapster;
using EntityFramework.Exceptions.Common;
using TimejApi.Helpers;
using TimejApi.Services.User;
using UserRole = TimejApi.Data.Entities.User.Role;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserEntityController : Controller
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
        /// Protected method to get User data
        /// </summary>
        /// <remarks>
        /// Acceptable for owners and MODERATORs
        /// </remarks>
        [HttpGet("{id}/details")]
        [Authorize]
        public async Task<ActionResult<UserPublicDto>> GetDetails(Guid id)
        {
            if (!User.SubEqualsOrInRole(id, nameof(UserRole.MODERATOR))) return Forbid();

            if (await _userService.DbContext.Users.FindAsync(id) is not User user)
                return NotFound();

            return Ok(user.Adapt<UserDto>());
        }


        /// <summary>
        /// Edits user entity. Enable for MODERATOR or entity owner
        /// </summary>
        /// <remarks>
        /// Acceptable for MODERATORs
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public async Task<ActionResult<UserDto>> Put(Guid id, UserRegister data)
        {
            var newUserModel = data.Adapt<User>();
            newUserModel.Id = id;
            User.TryGetSubAsGuid(out var moderatorId);

            try
            {
                _userService.DbContext.Users.Update(newUserModel);
                await _userService.DbContext.SaveChangesAsync();
                _logger.LogInformation("User ({}) has beed updated by Moderator ({})", newUserModel.Id, moderatorId);
                return newUserModel.Adapt<UserDto>();
            }
            catch (UniqueConstraintException exception)
            {
                _logger.LogWarning(exception, "Problem while Moderator ({}) tried to edit user ({})", moderatorId, newUserModel.Id);
                return Conflict();
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
            if (!User.TryGetSubAsGuid(out var callerId) || callerId != id
                && !User.IsInRole(nameof(UserRole.MODERATOR))) return Forbid();
            try
            {
                var user = await _userService.DbContext.Users.FindAsync(id);
                if (user == null) return NotFound();

                var newUserModel = await _userService.Edit(user, data);
                _logger.LogInformation("User ({}) edited user ({})", callerId, newUserModel.Id);

                return Ok(newUserModel.Adapt<UserDto>());
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
            User.TryGetSubAsGuid(out var moderatorId);

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
        }
    }
}
