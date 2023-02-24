using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Controllers
{
    [Route("api/building/{buildingNumber}/auditory")]
    [ApiController]
    public class AuditoryController : ControllerBase
    {
        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<Auditory[]>> GetAll(uint buildingNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPut("{number}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Auditory>> Put(uint number, AuditoryCreation auditory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Auditory>> Post(AuditoryCreation auditory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpDelete("{number}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult> Delete(uint number)
        {
            throw new NotImplementedException();
        }

    }
}
