using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("all")]
        public async Task<ActionResult<Subject[]>> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<Subject>> Post(SubjectCreation auditory)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<Subject>> Put(Guid id, SubjectCreation auditory)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
