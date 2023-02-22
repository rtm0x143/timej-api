using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        public record LessonGroup(uint? SubGroupNumber, uint GroupNumber);
        public record Teacher(Guid Id, string Fullname);

        public record Lesson(Guid Id, DateOnly Date, uint LessonNumber, string LessonType, 
            string Subject, LessonGroup[] Groups, Teacher Teacher);

        public record ScheduleDay(DateOnly Date, Lesson[] Lessons);

        [HttpGet]
        public async Task<ActionResult<ScheduleDay[]>> Get(
            [FromQuery] DateOnly beginDate,
            [FromQuery] DateOnly endDate,
            [FromQuery] uint? groupNumber, 
            [FromQuery] Guid? teacherId, 
            [FromQuery] [NotNullIfNotNull(nameof(buildingNumber))] uint? auditoryNumber,
            [FromQuery] [NotNullIfNotNull(nameof(auditoryNumber))] uint? buildingNumber,
            [FromQuery] bool isOnline = false) 
        {
            throw new NotImplementedException();
        }

        [HttpGet("default")]
        [Authorize]
        public async Task<ActionResult<ScheduleDay[]>> GetDefault()
        {
            throw new NotImplementedException();
        }

        public record LessonCreation(DateOnly Date, uint LessonNumber, Guid LessonId,
            Guid SubjectId, LessonGroup[] Groups, Guid TeacherId);

        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<Lesson>> Post(LessonCreation lesson)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<Lesson>> Rut(LessonCreation lesson)
        {
            throw new NotImplementedException();
        }
    }
}
