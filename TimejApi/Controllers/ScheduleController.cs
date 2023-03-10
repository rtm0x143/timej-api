using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;
using TimejApi.Helpers;
using TimejApi.Services;
using TimejApi.Services.User;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/scheduleService")]
    public class ScheduleController : Controller
    {
        private readonly ISchedule _scheduleService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public ScheduleController(ISchedule scheduleService, IAuthorizationService authorizationService, IUserService userService)
        {
            _scheduleService = scheduleService;
            _authorizationService = authorizationService;
            _userService = userService;
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
            return Ok(await _scheduleService.Get(beginDate, endDate, groupNumber, teacherId, auditoryId, isOnline));
        }

        /// <summary>
        /// Get default schedule of user from credentials
        /// </summary>
        [HttpGet("default")]
        [Authorize]
        public async Task<ActionResult<ScheduleDay[]>> GetDefault([FromQuery] DateOnly beginDate,
            [FromQuery] DateOnly endDate)
        {
            User.TryGetIdentifierAsGuid(out var guid);

            var userData = await  _userService.TryGet(guid);
            if (userData == null) { return NotFound($"User with id {guid} not found");}
            if(userData.StudentGroup is not null)
            {
                return Ok(await _scheduleService.Get(beginDate, endDate, userData.StudentGroup.Id, null, null));
            }
            return Ok(await _scheduleService.Get(beginDate, endDate, null, userData.Id, null));
        }

        /// <summary>
        /// Create lessons in a range
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LessonDto>> Post(LessonCreation lesson, [FromQuery] DateOnly? beginDate,
            [FromQuery] DateOnly? endDate)
        {
            await _authorizationService.AuthorizeAsync(User, lesson.AttendingGroups.Select(x => x.GroupId), "ScheduleEditor");
            try
            {
                return Ok(await _scheduleService.CreateLessons(lesson, beginDate, endDate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("replica/{id}")]
        public async Task<ActionResult<LessonDto>> Put(Guid replicaId, LessonCreation lesson)
        {
            await _authorizationService.AuthorizeAsync(User, lesson.AttendingGroups.Select(x => x.GroupId), "ScheduleEditor");
            try
            {
                return Ok(await _scheduleService.EditLessons(replicaId, lesson));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("replica/{id}")]
        public async Task<ActionResult> Delete(Guid replicaId)
        {
            var groups = await _scheduleService.GetAttendingGroups(replicaId);
            await _authorizationService.AuthorizeAsync(User, groups, "ScheduleEditor");
            try
            {
                await _scheduleService.DeleteLessons(replicaId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LessonDto>> PutSingle(Guid id, LessonCreation lesson)
        {
            await _authorizationService.AuthorizeAsync(User, lesson.AttendingGroups.Select(x => x.GroupId), "ScheduleEditor");
            try
            {
                return Ok(await _scheduleService.EditSingle(id, lesson));
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
        public async Task<ActionResult> DeleteSingle(Guid id)
        {
            var groups = await _scheduleService.GetAttendingGroups(id);
            await _authorizationService.AuthorizeAsync(User, groups, "ScheduleEditor");
            try
            {
                await _scheduleService.DeleteSingle(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
