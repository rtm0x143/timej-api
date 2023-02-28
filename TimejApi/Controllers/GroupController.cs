using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using UserRole = TimejApi.Data.Entities.User.Role;

namespace TimejApi.Controllers
{
    [ApiController]
    public class GroupController : ControllerBase
    {
        public GroupController()
        {
        }

        /// <summary>
        /// Get all groups related to that Faculty
        /// </summary>
        [HttpGet("api/faculty/{facultyId}/group/all")]
        public Task<ActionResult<GroupDto[]>> GetAll(Guid facultyId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get concrete Group
        /// </summary>
        [HttpGet("group/{id}")]
        public Task<ActionResult<GroupDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates new Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        [HttpGet("api/faculty/{facultyId}/group")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public Task<ActionResult<GroupDto>> Post(Guid facultyId, GroupCreaton group)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates some Group 
        /// </summary>
        /// <remarks>
        /// Requires MODERATOR role 
        /// </remarks>
        [HttpPut("api/faculty/{facultyId}/group/{id}")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public Task<ActionResult<GroupDto>> Put(Guid facultyId, Guid id, GroupCreaton group)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("api/faculty/{facultyId}/group/{id}")]
        [Authorize(Roles = nameof(UserRole.MODERATOR))]
        public Task<ActionResult<GroupDto>> Delete(Guid facultyId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
