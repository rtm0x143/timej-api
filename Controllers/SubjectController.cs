using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<Subject[]>> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<Subject>> Post(SubjectCreation subject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult<Subject>> Put(Guid id, SubjectCreation subject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleEditor")]
        public async Task<ActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
