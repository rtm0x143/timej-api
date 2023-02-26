using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        /// Get all buildings
        /// </summary>
        /// <response code="200"> Returns the building model by id</response>
        /// <response code="404"> Building was not found </response>
        [HttpGet("{buildingId}")]
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
        /// Returns all buildings from the entire database
        /// </summary>
        /// <response code="200"> Returns the building models</response>
        [HttpGet("all")]
        public async Task<ActionResult<Building[]>> GetAll()
        {
            return await _buildingService.GetAll();
        }

        /// <summary>
        /// Creates new building in a database
        /// </summary>
        /// <response code="200"> Returns the newly created building</response>
        /// <response code="403"> If not Moderator </response>
        /// <response code="401"> Not authorized </response>
        [HttpPost]
        //[Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
        public async Task<ActionResult<Building>> Post(BuildingCreation building)
        {
            return Ok(await _buildingService.Create(building));
        }

        /// <summary>
        /// Edits new building in a database
        /// </summary>
        /// <response code="200"> Returns the edited building</response>
        /// <response code="404"> Building was not found </response>
        /// <response code="403"> If not Moderator </response>
        /// <response code="401"> Not authorized </response>
        [HttpPut("{buildingId}")]
        //[Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
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
        /// <response code="200"> Deletes the building</response>
        /// <response code="403"> If not Moderator </response>
        /// <response code="401"> Not authorized </response>
        [HttpDelete("{buildingId}")]
        //[Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
        public async Task<ActionResult> Delete(Guid buildingId)
        {
            await _buildingService.DeleteIfExists(buildingId);
            return Ok();
        }
    }
}
