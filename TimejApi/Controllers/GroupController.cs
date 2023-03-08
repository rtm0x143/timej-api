using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;
using UserRole = TimejApi.Data.Entities.User.Role;

namespace TimejApi.Controllers
{
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ScheduleDbContext _context;

        public GroupController(ScheduleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all groups related to that Faculty
        /// </summary>
        [HttpGet("api/faculty/{facultyId}/group/all")]
        public Task<GroupDto[]> GetAll(Guid facultyId)
        {
            return _context.Groups.Where(g => g.Faculty.Id == facultyId)
                .ToArrayAsync()
                .ContinueWith(groupsTask => groupsTask.Result.Adapt<GroupDto[]>());
        }

        /// <summary>
        /// Get concrete Group
        /// </summary>
        [HttpGet("api/group/{id}")]
        public async Task<ActionResult<Group>> Get(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return NotFound();
            return Ok(group);
        }

        /// <returns>Collided Group if exist else null</returns>
        private Task<Group?> _checkGroupNumberFacultyCollision(Group target)
        {
            return _context.Groups
                .FirstOrDefaultAsync(g => g.FacultyId == target.FacultyId && g.GroupNumber == target.GroupNumber)
                .ContinueWith(t =>
                {
                    if (t.Result?.Id is Guid collisionId && collisionId != target.Id) return t.Result;
                    return null;
                });
        }

        /// <summary>
        /// Creates new Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        /// <response code="409">When new entity has collision with existing. Returns conflicting entity</response>
        [HttpPost("api/faculty/{facultyId}/group")]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(GroupDto))]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public async Task<ActionResult<GroupDto>> Post(Guid facultyId, GroupCreaton group)
        {
            var faculty = await _context.Faculties.FindAsync(facultyId);
            if (faculty == null) return NotFound("Unknown faculty");

            var new_group = group.Adapt(new Group() { Faculty = faculty, FacultyId = faculty.Id });
            if ((await _checkGroupNumberFacultyCollision(new_group)) is Group collision) 
                return Conflict(collision);

            var entry = _context.Groups.Add(new_group);
            await _context.SaveChangesAsync();
            return Ok(entry.Entity.Adapt<GroupDto>());
        }

        /// <summary>
        /// Updates some Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        /// <response code="409">When changed entity has collision with existing. Returns conflicting entity</response>
        [HttpPut("api/faculty/{facultyId}/group/{id}")]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(GroupDto))]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public async Task<ActionResult<GroupDto>> Put(Guid facultyId, Guid id, GroupCreaton group)
        {
            var faculty = await _context.Faculties.FindAsync(facultyId);
            if (faculty == null) return NotFound("Unknown faculty");

            var new_group = group.Adapt(new Group() { Id = id, Faculty = faculty, FacultyId = faculty.Id });
            if ((await _checkGroupNumberFacultyCollision(new_group)) is Group collision)
                return Conflict(collision.Adapt<GroupDto>());

            var entry = _context.Groups.Update(new_group);
            await _context.SaveChangesAsync();
            return Ok(entry.Entity.Adapt<GroupDto>());
        }

        [HttpDelete("api/group/{id}")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public Task<ActionResult> Delete(Guid id)
        {
            return _context.Groups.Where(g => g.Id == id)
                .ExecuteDeleteAsync()
                .ContinueWith<ActionResult>(task => task.Result == 0 ? NotFound() : Ok());
        }
    }
}
