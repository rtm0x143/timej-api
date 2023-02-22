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
        //Return token
        public async Task<ActionResult<string>> Login(UserLogin user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An method to create user by admin
        /// </summary>
        /// <param name="user"></param>
        [HttpPost("register")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult<User>> RegisterUser(UserRegister user)
        {
            throw new NotImplementedException();
        }

    }
}
