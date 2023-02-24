using Microsoft.AspNetCore.Mvc;
using TimejApi.Data.Entities;
using TimejApi.Data;
using TimejApi.Services;
using TimejApi.Services.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserAuthController : Controller
    {
        private readonly IJwtAuthentication _jwtAuthentication;

        public UserAuthController(IJwtAuthentication jwtAuthentication)
        {
            _jwtAuthentication = jwtAuthentication;
        }

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

    }
}