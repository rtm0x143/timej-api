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
    }
}
