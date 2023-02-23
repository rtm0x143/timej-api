using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using TimejApi.Data;
using TimejApi.Data.Dtos;

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
    public class ScheduleEditorHandler : AuthorizationHandler<FacultyEditorRequirement, LessonCreation>
    {
        private readonly ScheduleDbContext _context;

        public ScheduleEditorHandler(ScheduleDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FacultyEditorRequirement requirement, LessonCreation resource)
        {
            if (context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value is not string sub
                || Guid.TryParse(sub, out var userId))
            {
                context.Fail(new AuthorizationFailureReason(this, "User's claim 'sub' was invalid or unspecified"));
                return;
            }

            var allowedGroups = await _context.UserEditFacultyPermissions
                .Where(p => p.EditorId == userId)
                .SelectMany(p => p.AllowedFaculty.Groups)
                .Select(g => g.Id)
                .ToArrayAsync();
            
            foreach (var group in resource.Groups)
            {
                if (!allowedGroups.Contains(group.Id)) return;
            }

            context.Succeed(requirement);
        }
    }
}
