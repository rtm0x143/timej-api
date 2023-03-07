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
    [Route("api/faculty")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly ScheduleDbContext _context;

        public FacultyController(ScheduleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Faculty entity
        /// </summary>
        /// <response code="404">When specified id is unknown</response>
        [HttpGet("{id}")]
        public Task<ActionResult<Faculty>> Get(Guid id)
        {
            return _context.Faculties.FindAsync(id).AsTask()
                .ContinueWith<ActionResult<Faculty>>(t => t.Result == null ? NotFound() : Ok(t.Result));
        }


        /// <summary>
        /// Get all Faculties 
        /// </summary>
        [HttpGet("all")]
        public Task<Faculty[]> GetAll()
        {
            return _context.Faculties.ToArrayAsync();
        }

        /// <summary>
        /// Create new Faculty entity
        /// </summary>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Caller is not MODERATOR role</response>
        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.MODERATOR))]
        public Task<Faculty> Post(FacultyCreation faculty)
        {
            var entry = _context.Faculties.Add(faculty.Adapt<Faculty>());
            return _context.SaveChangesAsync()
                .ContinueWith(t => entry.Entity);
        }

        /// <summary>
        /// Update Faculty entity
        /// </summary>
        /// <response code="404">When id is unknown</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Caller is not MODERATOR role</response>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRoles.MODERATOR))]
        public async Task<ActionResult<Faculty>> Put(Guid id, FacultyCreation faculty)
        {
            var entity = await _context.Faculties.FindAsync(id);
            if (entity == null) return NotFound();

            var entry = _context.Faculties.Update(faculty.Adapt(entity));
            await _context.SaveChangesAsync();
            return Ok(entry.Entity);
        }

        /// <summary>
        /// Delete Faculty entity
        /// </summary>
        /// <response code="404">When id is unknown</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Caller is not MODERATOR role</response>
        /// <response code="204">When deletion succeeded</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRoles.MODERATOR))]
        public Task<ActionResult> Delete(Guid id)
        {
            return _context.Faculties.Where(f => f.Id == id)
                .ExecuteDeleteAsync()
                .ContinueWith<ActionResult>(task => task.Result == 0 ? NotFound() : NoContent());
        }
    }
}
