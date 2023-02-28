using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<GroupDto>> Get(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return NotFound();
            return Ok(group.Adapt<GroupDto>());
        }

        /// <summary>
        /// Creates new Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        [HttpPost("api/faculty/{facultyId}/group")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public async Task<ActionResult<GroupDto>> Post(Guid facultyId, GroupCreaton group)
        {
            var faculty = await _context.Faculties.FindAsync(facultyId);
            if (faculty == null) return NotFound("Unknown faculty");

            var entry = _context.Groups.Add(group.Adapt(new Group() { Faculty = faculty }));
            await _context.SaveChangesAsync();
            return Ok(entry.Entity.Adapt<GroupDto>());
        }

        /// <summary>
        /// Updates some Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        [HttpPut("api/faculty/{facultyId}/group/{id}")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public async Task<ActionResult<GroupDto>> Put(Guid facultyId, Guid id, GroupCreaton group)
        {
            var faculty = await _context.Faculties.FindAsync(facultyId);
            if (faculty == null) return NotFound("Unknown faculty");

            var entry = _context.Groups.Update(
                group.Adapt(new Group() { Faculty = faculty, Id = id }));
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
