using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Entities;
using TimejApi.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using EntityFramework.Exceptions.Common;
using TimejApi.Helpers;
using TimejApi.Services;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserEntityController : Controller
    {
        private readonly ScheduleDbContext _context;
        private readonly ILogger<UserEntityController> _logger;
        private readonly IPasswordHasher _passwordHasher;

        public UserEntityController(ScheduleDbContext context, ILogger<UserEntityController> logger, IPasswordHasher passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Public method to get some User data
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPublicDto>> Get(Guid id)
        {
            if (await _context.Users.FindAsync(id) is not User user)
                return NotFound();

            return Ok(user.Adapt<UserPublicDto>());
        }

        private async Task<ActionResult<UserDto>> UpdateUserChecked(User user)
        {
            var newUserEntry = _context.Users.Update(user);
            User.TryGatSubAsGuid(out var moderatorId);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (UniqueConstraintException)
            {
                _logger.LogWarning("Moderator ({}) tried to edit unknown user ({})", moderatorId, user.Id);
                return NotFound();
            }

            _logger.LogInformation("User ({}) has beed updated by Moderator ({})", user.Id, moderatorId);
            return Ok(newUserEntry.Entity.Adapt<UserDto>());
        }

        /// <summary>
        /// Edits user entity. Enable for MODERATOR or entity owner
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
        public async Task<ActionResult<UserDto>> Put(Guid id, UserData data)
        {
            var newUserModel = data.Adapt<User>();
            newUserModel.Id = id;

            return await UpdateUserChecked(newUserModel);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Patch(Guid id, UserPublicData data)
        {
            if (!User.TryGatSubAsGuid(out var userId) || userId != id
                || !User.IsInRole(nameof(Data.Entities.User.Role.MODERATOR))) return Forbid();

            var newUserModel = data.Adapt<User>();
            newUserModel.Id = id;

            return await UpdateUserChecked(newUserModel);
        }

        /// <summary>
        /// An method to create user by admin
        /// </summary>
        [HttpPost("register")]
        [Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
        public async Task<ActionResult<UserDto>> RegisterUser(UserRegister user)
        {
            if (user.Group != null && !user.Roles.Contains(Data.Entities.User.Role.STUDENT))
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest,
                               detail: "User related to some Group should also have \"STUDENT\" role");
            }

            User.TryGatSubAsGuid(out var moderatorId);

            try
            {
                var userModel = user.Adapt<User>();
                if (userModel.StudentGroup != null)
                    _context.Groups.Entry(userModel.StudentGroup).State = EntityState.Unchanged;
                userModel.PasswordHash = _passwordHasher.HashPassword(user.Password);

                var entry = _context.Users.Add(userModel);
                _context.SaveChanges();

                _logger.LogInformation("New User ({}) with roles [{}] has been created by Moderator ({}).", 
                    entry.Entity.Id, string.Join(", ", user.Roles), moderatorId);

                return Ok(entry.Entity.Adapt<UserDto>());
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
