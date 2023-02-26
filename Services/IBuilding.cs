using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Services
{
    public interface IBuilding
    {
        public Task<Building?> Get(Guid buildingId);

        public Task<Building[]> GetAll();

        public Task<Building> Create(BuildingCreation building);

        public Task<Building?> Edit(Guid buildingId, BuildingCreation building);

        public Task DeleteIfExists(Guid buildingId);

    }
}
