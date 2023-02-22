using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Controllers
{
    [Route("api/building/{buildingNumber}/auditory")]
    [ApiController]
    public class AuditoryController : ControllerBase
    {
        [HttpGet("{number}")]
        public async Task<ActionResult<Auditory>> Get(uint number, uint buildingNumber)
        {
            throw new NotImplementedException();
        }

        [HttpGet("all")]
        public async Task<ActionResult<Auditory[]>> GetAll(uint buildingNumber)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{number}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Auditory>> Put(uint number, AuditoryCreation auditory)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Auditory>> Post(AuditoryCreation auditory)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{number}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task Delete(uint number)
        {
            throw new NotImplementedException();
        }

    }
}
