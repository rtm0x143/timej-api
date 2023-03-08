using EntityFramework.Exceptions.Common;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Entities;
using TimejApi.Helpers;
using TimejApi.Services.User;
using UserRoles = TimejApi.Data.Entities.User.Role;

namespace TimejApi.Controllers;

/// <summary>
/// Contains specific operations which can be performed on User by MODERATOR
/// </summary>
public class UserModeratorController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserModeratorController> _logger;

    public UserModeratorController(IUserService userService, ILogger<UserModeratorController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Grant some user permission to edit schedule of some faculty
    /// </summary>
    /// <response code="404">Not found some entity</response>
    /// <response code="409">Already exist</response>
    /// <response code="403">Caller is not in MODERATOR role</response>
    /// <returns>Collection of allowed faculties</returns>
    [HttpPost("{id}/edit-permission/{facultyId}")]
    [Authorize(Roles = nameof(UserRoles.MODERATOR))]
    public async Task<ActionResult<Faculty[]>> PostEditPermission(Guid id, Guid facultyId)
    {
        if (await _userService.TryGet(id) is not User user) return NotFound("User's Id is unknown");
        if (user.Roles.FirstOrDefault(u => u.Role == UserRoles.SCHEDULE_EDITOR) == null)
            return Problem(statusCode: StatusCodes.Status400BadRequest,
                           title: "Invalid target user",
                           detail: "User should be in group EDITOR to recieve edit permissions");

        var result = await _userService.TryGrantEditPermission(id , facultyId);
        if (result.Ensure(out var faculties, out var exception)) return Ok(faculties.ToArray());

        return exception switch
        {
            KeyNotFoundException => NotFound("Faculty is unknown"),
            ArgumentException => Conflict("Already exist"),
            _ => Problem(),
        };
    }

    [HttpDelete("{id}/edit-permission/{facultyId}")]
    [Authorize(Roles = nameof(UserRoles.MODERATOR))]
    public Task<ActionResult> DeleteEditPermission(Guid id, Guid facultyId)
    {
        return _userService.RevokeEditPermission(id, facultyId).AsTask()
            .ContinueWith<ActionResult>(t =>
            {
                if (t.Result.Ensure(out var perm, out var exception)) return NoContent();
                return exception switch
                {
                    KeyNotFoundException => NotFound(),
                    _ => Problem(),
                };
            });
    }

    /// <summary>
    /// An method to register new user
    /// </summary>
    /// <remarks>
    /// Acceptable for MODERATORs
    /// </remarks>
    [HttpPost("register")]
    [Authorize(Roles = nameof(UserRoles.MODERATOR))]
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
    /// Edits user entity
    /// </summary>
    /// <remarks>
    /// Acceptable for MODERATORs
    /// </remarks>
    [HttpPut("{id}")]
    [Authorize(Roles = nameof(UserRoles.MODERATOR))]
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
}

