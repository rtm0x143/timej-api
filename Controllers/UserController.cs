using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Dtos;
using TimejApi.Data.Entities;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(UserLogin user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An method to create user by admin
        /// </summary>
        /// <param name="user"></param>
        [HttpPost("register")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult<AuthResult>> RegisterUser(UserRegister user)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult<UserDto>> Edit(Guid id, UserBase user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Should only be called when user ID is an ID of editor
        /// </summary>
        [HttpPost("{userId}")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult> AddFaculties(Guid userId, Faculty[] faculties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Should only be called when user ID is an ID of editor
        /// </summary>
        [HttpPut("{userId}")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult> RemoveFaculties(Guid userId, Faculty[] faculties)
        {
            throw new NotImplementedException();
        }
    }
}
