using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResult>> Refresh(AuthResult auth)
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
        // [Authorize]
        //      TODO : should it be public ?
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edits user entity. Enable for MODERATOR or entity owner
        /// </summary>
        [HttpPut("{id}")]
        [Authorize] 
        public async Task<ActionResult<UserDto>> Put(Guid id, UserData user)
        {
            throw new NotImplementedException();
        }
    }
}
