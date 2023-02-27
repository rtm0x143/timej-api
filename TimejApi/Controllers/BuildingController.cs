using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Entities;
using TimejApi.Data.Dtos;
using TimejApi.Services;
using TimejApi.Data;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace TimejApi.Controllers
{

    [ApiController]
    [Route("api/building")]
    public class BuildingController : Controller
    {

        private readonly ScheduleDbContext _context;

        public BuildingController(ScheduleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all buildings
        /// </summary>
        /// <response code="200"> Returns the building model by id</response>
        /// <response code="404"> Building was not found </response>
        [HttpGet("{buildingId}")]
        public async Task<ActionResult<Building>> Get(Guid buildingId)
        {
            var building = await _context.Buildings.FindAsync(buildingId);
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
            return Ok(await _context.Buildings.ToArrayAsync());
        }

        /// <summary>
        /// Creates new building in a database
        /// </summary>
        /// <response code="200"> Returns the newly created building</response>
        /// <response code="403"> If not Moderator </response>
        /// <response code="401"> Not authorized </response>
        [HttpPost]
        [Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
        public async Task<ActionResult<Building>> Post(BuildingCreation building)
        {
            var result = building.Adapt<Building>();
            await _context.Buildings.AddAsync(result);
            await _context.SaveChangesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Edits new building in a database
        /// </summary>
        /// <response code="200"> Returns the edited building</response>
        /// <response code="404"> Building was not found </response>
        /// <response code="403"> If not Moderator </response>
        /// <response code="401"> Not authorized </response>
        [HttpPut("{buildingId}")]
        [Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
        public async Task<ActionResult<Building>> Put(Guid buildingId, BuildingCreation building)
        {
            var result = await _context.Buildings.FindAsync(buildingId);
            result = building.Adapt(result);
            await _context.SaveChangesAsync();
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
        /// <response code="404"> Building was not found </response>
        /// <response code="403"> If not Moderator </response>
        /// <response code="401"> Not authorized </response>
        [HttpDelete("{buildingId}")]
        [Authorize(Roles = nameof(Data.Entities.User.Role.MODERATOR))]
        public async Task<ActionResult> Delete(Guid buildingId)
        {
            var result = await _context.Buildings.Where(x => x.Id == buildingId).ExecuteDeleteAsync();
            if(result>0)
                return Ok();
            return NotFound($"Building with id {buildingId} was not found");
        }
    }
}
