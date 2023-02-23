﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpDelete("{id}")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Should only be called when user ID is an ID of editor
        /// </summary>
        [HttpPost("{userId}")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult> AddFaculties(Guid userId, Guid[] facultyIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Should only be called when user ID is an ID of editor
        /// </summary>
        [HttpPut("{userId}")]
        // TODO: Add policy [Authorize(Policy = "Admin")]
        public async Task<ActionResult> RemoveFaculties(Guid userId, Guid[] facultyIds)
        {
            throw new NotImplementedException();
        }
    }
}
