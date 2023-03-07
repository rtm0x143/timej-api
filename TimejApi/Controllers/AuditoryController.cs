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
    [ApiController]
    public class AuditoryController : ControllerBase
    {
        private readonly ScheduleDbContext _context;

        public AuditoryController(ScheduleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Auditories related to specified building
        /// </summary>
        [HttpGet("api/building/{buildingId}/auditory/all")]
        public Task<Auditory[]> GetAll(Guid buildingId)
        {
            return _context.Auditories.Where(a => a.Building.Id == buildingId)
                .ToArrayAsync();
        }

        /// <summary>
        /// Get concrete Auditory 
        /// </summary>
        /// <response code="404">Not found</response>
        [HttpGet("api/auditory/{auditoryId}")]
        public Task<ActionResult<Auditory>> Get(Guid auditoryId)
        {
            return _context.Auditories.FindAsync(auditoryId).AsTask()
                .ContinueWith<ActionResult<Auditory>>(t => t.Result == null ? NotFound() : Ok(t.Result));
        }

        /// <summary>
        /// Update Auditory entity
        /// </summary>
        /// <response code="404">Not found</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">When caller is not MODERATOR</response>
        [HttpPut("api/auditory/{auditoryId}")]
        [Authorize(Roles = $"{nameof(UserRoles.MODERATOR)}")]
        public async Task<ActionResult<Auditory>> Put(Guid auditoryId, AuditoryCreation auditoryCreation)
        {
            var auditory = await _context.Auditories.FindAsync(auditoryId);
            if (auditory == null) return NotFound();

            var entry = _context.Auditories.Update(auditoryCreation.Adapt(auditory));;
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        /// <summary>
        /// Create new Auditory entity
        /// </summary>
        /// <response code="401">Not authorized</response>
        /// <response code="403">When caller is not MODERATOR</response>
        [HttpPost("api/auditory")]
        [Authorize(Roles = $"{nameof(UserRoles.MODERATOR)}")]
        public Task<ActionResult<Auditory>> Post(AuditoryCreation auditory)
        {
            var entry = _context.Auditories.Add(auditory.Adapt<Auditory>());
            return _context.SaveChangesAsync()
                .ContinueWith<ActionResult<Auditory>>(t => Ok(entry.Entity));
        }

        /// <summary>
        /// Deletes Auditory entity
        /// </summary>
        /// <response code="401">Not authorized</response>
        /// <response code="403">When caller is not MODERATOR</response>
        /// <response code="404">When "auditoryId" is unknown</response>
        /// <responce code="204">If succeeded</responce>
        [HttpDelete("api/auditory{auditoryId}")]
        [Authorize(Policy = "SheduleModerator")]
        public Task<ActionResult> Delete(Guid auditoryId)
        {
            return _context.Auditories.Where(a => a.Id == auditoryId)
                .ExecuteDeleteAsync()
                .ContinueWith<ActionResult>(t => t.Result == 0 ? NotFound() : NoContent());
        }

    }
}
