using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using TimejApi.Data;
using TimejApi.Data.Dtos;
using TimejApi.Helpers;

namespace TimejApi.Services.Auth.Policies
{
    public class FacultyEditorRequirement : IAuthorizationRequirement
    {
    }

    /// <summary>
    /// Handler to check if User has permission to edit schedule related to some faculty.
    /// </summary>
    /// <remarks>
    /// It does not include role check.
    /// </remarks>
    public class ScheduleEditorHandler : AuthorizationHandler<FacultyEditorRequirement, IEnumerable<Guid>>
    {
        private readonly ScheduleDbContext _context;

        public ScheduleEditorHandler(ScheduleDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FacultyEditorRequirement requirement, IEnumerable<Guid> resource)
        {
            if (context.User.IsInRole(nameof(Data.Entities.User.Role.MODERATOR)))
            {
                context.Succeed(requirement);
                return;
            }
            
            if (!context.User.TryGetIdentifierAsGuid(out var userId))
            {
                context.Fail(new AuthorizationFailureReason(this, "User's claim 'sub' was invalid or unspecified"));
                return;
            }

            var allowedGroups = await _context.UserEditFacultyPermissions
                .Where(p => p.EditorId == userId)
                .SelectMany(p => p.AllowedFaculty.Groups)
                .Select(g => g.Id)
                .ToArrayAsync();
            
            foreach (var groupId in resource)
            {
                if (!allowedGroups.Contains(groupId)) return;
            }

            context.Succeed(requirement);
        }
    }
}
