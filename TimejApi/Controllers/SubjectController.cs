using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimejApi.Data;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;
using UserRoles = TimejApi.Data.Entities.User.Role;

namespace TimejApi.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ScheduleDbContext _context;

        public SubjectController(ScheduleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get concrete Subjetc
        /// </summary>
        /// <response code="404">When id is unknown</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> Get(Guid id)
        {
            var sub = await _context.Subjects.FindAsync(id);
            if (sub == null) return NotFound();
            return Ok(sub);
        }

        /// <summary>
        /// Get all existing Subjects 
        /// </summary>
        [HttpGet("all")]
        public Task<Subject[]> GetAll()
        {
            return _context.Subjects.ToArrayAsync();
        }

        /// <summary>
        /// Create new Subject 
        /// </summary>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Caller has not MODERATOR, not EDITOR role</response>
        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRoles.MODERATOR)},{nameof(UserRoles.SCHEDULE_EDITOR)}")]
        public Task<Subject> Post(SubjectCreation subject)
        {
            var entry = _context.Subjects.Add(subject.Adapt<Subject>());
            return _context.SaveChangesAsync().ContinueWith(t => entry.Entity);
        }

        /// <summary>
        /// Create new Subject 
        /// </summary>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Caller has not MODERATOR, not EDITOR role</response>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(UserRoles.MODERATOR)},{nameof(UserRoles.SCHEDULE_EDITOR)}")]
        public async Task<ActionResult<Subject>> Put(Guid id, SubjectCreation subjectCreation)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return NotFound();
            var entry = _context.Subjects.Update(subjectCreation.Adapt(subject));
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        /// <summary>
        /// Create new Subject 
        /// </summary>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Caller has not MODERATOR, not EDITOR role</response>
        /// <response code="204">When deletion succeeded</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UserRoles.MODERATOR)},{nameof(UserRoles.SCHEDULE_EDITOR)}")]
        public Task<ActionResult> Delete(Guid id)
        {
            return _context.Subjects.Where(s => s.Id == id)
                .ExecuteDeleteAsync()
                .ContinueWith<ActionResult>(t => t.Result == 0 ? NotFound() : NoContent());
        }

        /// <summary>
        /// Get all existing types of lessons 
        /// </summary>
        [HttpGet("types/all")]
        public async Task<LessonType[]> GetAllTypes()
        {
            return await  _context.LessonTypes.ToArrayAsync();
        }
    }
}
