using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;
using TimejApi.Services;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        private readonly ISchedule _schedule;

        public ScheduleController(ISchedule schedule)
        {
            _schedule = schedule;
        }

        [HttpGet]
        public async Task<ActionResult<ScheduleDay[]>> Get(
            [FromQuery] DateOnly beginDate,
            [FromQuery] DateOnly endDate,
            [FromQuery] Guid? groupNumber,
            [FromQuery] Guid? teacherId,
            [FromQuery] uint? buildingNumber,
            [FromQuery] uint? auditoryNumber,
            [FromQuery] bool isOnline = false)
        {
            try { 
            return Ok(await _schedule.Get(beginDate, endDate, groupNumber, teacherId, buildingNumber, auditoryNumber, isOnline));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("default")]
        [Authorize]
        public async Task<ActionResult<ScheduleDay[]>> GetDefault([FromQuery] DateOnly beginDate,
            [FromQuery] DateOnly endDate)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> Post(LessonCreation lesson, [FromQuery] DateOnly? beginDate,
            [FromQuery] DateOnly? endDate)
        {
            throw new NotImplementedException();
        }

        [HttpPut("replica/{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> Put(Guid replicaId, LessonCreation lesson, [FromQuery] DateOnly? beginDate,
            [FromQuery] DateOnly? endDate)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("replica/{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult> Delete(Guid replicaId, [FromQuery] DateOnly? beginDate,
            [FromQuery] DateOnly? endDate)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> PutSingle(Guid id, LessonCreation lesson)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult> DeleteSingle(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
