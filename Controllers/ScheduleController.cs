using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Dtos;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
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
        public async Task<ActionResult<ScheduleDay[]>> GetDefault([FromQuery] DateOnly beginDate,
            [FromQuery] DateOnly endDate)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> Post(LessonCreation lesson)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<LessonDto>> Put(Guid id, LessonCreation lesson)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
