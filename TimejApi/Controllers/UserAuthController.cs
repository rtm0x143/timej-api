using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TimejApi.Services.Auth;
using TimejApi.Services.User;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserAuthController : Controller
    {
        private readonly IJwtAuthentication _jwtAuthentication;
        private readonly IUserService _userService;
        private readonly ILogger<UserAuthController> _logger;

        public UserAuthController(IJwtAuthentication jwtAuthentication, IUserService userService, ILogger<UserAuthController> logger)
        {
            _jwtAuthentication = jwtAuthentication;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates user 
        /// </summary>
        /// <returns>Token pair JWT and associated Refresh token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(UserLogin userLogin)
        {
            if (!(await _userService.TryLogin(userLogin)).Ensure(out var user, out var errors))
                return ValidationProblem(statusCode: StatusCodes.Status401Unauthorized, modelStateDictionary: errors);

            var jwt = _jwtAuthentication.BuildToken(User.Claims);
            var result = new AuthResult(_jwtAuthentication.WriteToken(jwt), _jwtAuthentication.CreateRefreshTokenFor(jwt));
            _logger.LogInformation("Built JWT ({}) and Refresh for User ({})", jwt.Id, user.Id);
            return result;
        }

        /// <summary>
        /// [NOT IMPLEMENTED]
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResult>> Refresh(AuthResult auth)
        {
            throw new NotImplementedException();
        }

    }
}