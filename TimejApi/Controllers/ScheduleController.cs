using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
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

            var userData = await _userService.TryGet(guid);
            if (userData == null) { return NotFound($"User with id {guid} not found"); }
            if (userData.StudentGroup is not null)
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
            [FromQuery] DateOnly? endDate, uint repeatInterval)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, lesson.AttendingGroups.Select(x => x.GroupId), "ScheduleEditor")).Succeeded)
            {
                return Problem(statusCode: StatusCodes.Status403Forbidden,
                               detail: "You don't have enough permissions to perform this action");
            }
            if (await _userService.IsTeacher(lesson.TeacherId))
            {
                return BadRequest($"Teacher with id {lesson.TeacherId} is invalid");
            }
            try
            {
                return Ok(await _scheduleService.CreateLessons(lesson, beginDate, endDate, repeatInterval));
            }
            catch( ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch ( DbUpdateException e )
            {
                return BadRequest("Data you tried to insert was invalid");
            }
        }

        [HttpPut("replica/{id}")]
        public async Task<ActionResult<LessonDto>> Put(Guid replicaId, LessonEdit lesson)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, lesson.AttendingGroups.Select(x => x.GroupId), "ScheduleEditor")).Succeeded)
            {
                return Problem(statusCode: StatusCodes.Status403Forbidden,
                               detail: "You don't have enough permissions to perform this action");
            }
            if (await _userService.IsTeacher(lesson.TeacherId))
            {
                return BadRequest($"Teacher with id {lesson.TeacherId} is invalid");
            }
            try
            {
                return Ok(await _scheduleService.EditLessons(replicaId, lesson));
            }
            catch
            {
                return BadRequest("Data you tried to insert was invalid");
            }
        }

        [HttpDelete("replica/{id}")]
        public async Task<ActionResult> Delete(Guid replicaId)
        {
            var groups = await _scheduleService.GetAttendingGroups(replicaId);
            if (!(await _authorizationService.AuthorizeAsync(User, groups, "ScheduleEditor")).Succeeded)
            {
                return Problem(statusCode: StatusCodes.Status403Forbidden,
                               detail: "You don't have enough permissions to perform this action");
            }
            try
            {
                await _scheduleService.DeleteLessons(replicaId);
                return Ok();
            }
            catch
            {
                return BadRequest("Data you tried to insert was invalid");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LessonDto>> PutSingle(Guid id, LessonEdit lesson)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, lesson.AttendingGroups.Select(x => x.GroupId), "ScheduleEditor")).Succeeded)
            {
                return Problem(statusCode: StatusCodes.Status403Forbidden, detail: "You don't have enough permissions to perform this action");
            }
            if (await _userService.IsTeacher(lesson.TeacherId))
            {
                return BadRequest($"Teacher with id {lesson.TeacherId} is invalid");
            }
            try
            {
                return Ok(await _scheduleService.EditSingle(id, lesson));
            }
            catch { return BadRequest("Data you tried to insert was invalid"); }
        }

        /// <summary>
        /// Delete single lesson by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSingle(Guid id)
        {
            var groups = await _scheduleService.GetAttendingGroups(id);
            if (!(await _authorizationService.AuthorizeAsync(User, groups, "ScheduleEditor")).Succeeded)
            {
                return Problem(statusCode: StatusCodes.Status403Forbidden, detail: "You don't have enough permissions to perform this action");
            }
            try
            {
                await _scheduleService.DeleteSingle(id);
                return Ok();
            }
            catch { return BadRequest("Data you tried to insert was invalid"); }
        }
    }
}
