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
            [FromQuery] Guid? auditoryId,
            [FromQuery] bool isOnline = false)
        {
                return Ok(await _schedule.Get(beginDate, endDate, groupNumber, teacherId, auditoryId, isOnline));
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpGet("default")]
        [Authorize]
        public async Task<ActionResult<ScheduleDay[]>> GetDefault([FromQuery] DateOnly beginDate,
            [FromQuery] DateOnly endDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create lessons in a range
        /// </summary>
        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> Post(LessonCreation lesson, [FromQuery] DateOnly? beginDate,
            [FromQuery] DateOnly? endDate)
        {
            try
            {
                return Ok(await _schedule.CreateLessons(lesson, beginDate, endDate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("replica/{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> Put(Guid replicaId, LessonCreation lesson)
        {
            try
            {
                return Ok(await _schedule.EditLessons(replicaId, lesson));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("replica/{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult> Delete(Guid replicaId)
        {
            try
            {
                await _schedule.DeleteLessons(replicaId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> PutSingle(Guid id, LessonCreation lesson)
        {
            try
            {
                return Ok(await _schedule.EditSingle(id, lesson));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete single lesson by id
        /// </summary>
        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult> DeleteSingle(Guid id)
        {
            try
            {
                await _schedule.DeleteSingle(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
