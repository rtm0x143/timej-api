using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TimejApi.Data.Entities;
using TimejApi.Data.Dtos;
using TimejApi.Services;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/building")]
    public class BuildingController : Controller
    {

        private readonly IBuilding _buildingService;

        public BuildingController(IBuilding buildingService)
        {
            _buildingService = buildingService;
        }

        /// <summary>
        /// Returns the building model by id,
        /// if building not found returns 404
        /// </summary>
        [HttpGet("{number}")]
        public async Task<ActionResult<Building>> Get(Guid buildingId)
        {
            var building = await _buildingService.Get(buildingId);
            if (building == null)
            {
                return NotFound($"Building with id {buildingId} was not found");
            }
            return Ok(building);
        }

        /// <summary>
        /// Returns all building models from the entire database
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<Building[]>> GetAll()
        {
            return await _buildingService.GetAll();
        }

        /// <summary>
        /// Creates new building in a database
        /// if creation was successfull returns corresponding building model,
        /// otherwise 401
        /// </summary>
        [HttpPost]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Building>> Post(BuildingCreation building)
        {
            return await _buildingService.Create(building);
        }

        /// <summary>
        /// Edits new building in a database
        /// if edit was successfull returns corresponding building model,
        /// if building was not found return 404,
        /// otherwise 401
        /// </summary>
        [HttpPut("{number}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult<Building>> Put(Guid buildingId, BuildingCreation building)
        {
            var result = await _buildingService.Edit(buildingId, building);
            if (result == null)
            {
                return NotFound($"Building with id {buildingId} was not found");
            }
            return Ok(result);
        }

        /// <summary>
        /// Deletes the building if it exists
        /// </summary>
        [HttpDelete("{number}")]
        // TODO: Add policy [Authorize(Policy = "SheduleModerator")]
        public async Task<ActionResult> Delete(Guid buildingId)
        {
            await _buildingService.DeleteIfExists(buildingId);
            return Ok();
        }
    }
}
