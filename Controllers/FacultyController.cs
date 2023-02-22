using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Controllers
{
    [Route("api/faculty")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Faculty>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("all")]
        public async Task<ActionResult<Faculty[]>> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Faculty>> Post(FacultyCreation auditory)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Faculty>> Put(Guid id, FacultyCreation auditory)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
