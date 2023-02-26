using Mapster;
using Microsoft.EntityFrameworkCore;
using TimejApi.Data;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Services
{
    public class BuildingService : IBuilding
    {
        private readonly ScheduleDbContext _context;
        private readonly ILogger<BuildingService> _logger;

        public BuildingService(ILogger<BuildingService> logger, ScheduleDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Building> Create(BuildingCreation building)
        {
            var result = building.Adapt<Building>();
            await _context.Buildings.AddAsync(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Building?> Edit(Guid buildingId, BuildingCreation building)
        {
            var result = await _context.Buildings.FindAsync(buildingId);
            if (result == null) return null;
            result = building.Adapt(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Building?> Get(Guid buildingId)
        {
            return await _context.Buildings.FindAsync(buildingId);
        }

        public async Task<Building[]> GetAll()
        {
            return await _context.Buildings.ToArrayAsync();
        }

        public async Task DeleteIfExists(Guid buildingId)
        {
            await _context.Buildings.Where(x => x.Id == buildingId).ExecuteDeleteAsync();
        }
    }
}
