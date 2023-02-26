using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Entities;
using TimejApi.Data.Dtos;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/building")]
    public class BuildingController : Controller
    {
        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpGet("{number}")]
        public async Task<ActionResult<Building>> Get(uint number)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<Building[]>> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Building>> Post(BuildingCreation building)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPut("{number}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<LessonDto>> Put(uint number, BuildingCreation building)
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
