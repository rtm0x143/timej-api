using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using UserRole = TimejApi.Data.Entities.User.Role;

namespace TimejApi.Controllers
{
    [Route("api/faculty/{facultyId}/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        public GroupController()
        {
        }

        /// <summary>
        /// Get all groups related to that Faculty
        /// </summary>
        [HttpGet]
        public Task<Action<GroupDto[]>> GetAll(Guid facultyId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get concrete Group
        /// </summary>
        [HttpGet("{id}")]
        public Task<Action<GroupDto>> Get(Guid facultyId, Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates new Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public Task<Action<GroupDto>> Post(Guid facultyId, GroupCreaton group)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates some Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public Task<Action<GroupDto>> Put(Guid facultyId, Guid id, GroupCreaton group)
        {
            throw new NotImplementedException();
        }
    }
}
