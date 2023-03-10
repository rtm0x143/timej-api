using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimejApi.Helpers;
using TimejApi.Services.Auth;
using TimejApi.Services.User;

namespace TimejApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserAuthController : Controller
    {
        private readonly IJwtAuthentication _jwtAuthentication;
        private readonly IRefreshTokenService _refresh;
        private readonly IUserService _userService;
        private readonly ILogger<UserAuthController> _logger;

        public UserAuthController(IJwtAuthentication jwtAuthentication, IRefreshTokenService refresh,
            IUserService userService, ILogger<UserAuthController> logger)
        {
            _jwtAuthentication = jwtAuthentication;
            _refresh = refresh;
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

            var jwt = _jwtAuthentication.BuildToken(user.CreateClaims());
            var result = new AuthResult(_jwtAuthentication.WriteToken(jwt),
                                        await _refresh.CreateRefreshTokenFor(user.Id, Guid.Parse(jwt.Id)));
            _logger.LogInformation("Built JWT ({}) and Refresh token for User ({})", jwt.Id, user.Id);
            return Ok(result);
        }

        /// <summary>
        /// Refresh access token 
        /// </summary>
        /// <response code="400">When given tokens invalid</response>
        /// <response code="401">When refresh token already used</response>
        /// <response code="404">When token's subject is unknown</response>
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResult>> Refresh(AuthResult auth)
        {
            if (!new JwtSecurityTokenHandler().CanReadToken(auth.AccessToken))
                return BadRequest(nameof(auth.AccessToken));

            var jwt = new JwtSecurityToken(auth.AccessToken);
            if (!Guid.TryParse(jwt.Subject, out var userId)
                || !Guid.TryParse(jwt.Id, out var accessId)) return BadRequest();

            if (!await _refresh.UseRefreshToken(auth.RefreshToken))
            {
                await _refresh.RevokeAll(userId);
                return Problem(statusCode: StatusCodes.Status401Unauthorized,
                               title: "Invalid refresh token",
                               detail: "That token could be already used. " +
                                       "It could mean that third person stolen and used your tokens" +
                                       "Now all refresh tokens droped");
            }

            var user = await _userService.TryGet(userId);
            if (user == null) return NotFound();

            var newJwt = _jwtAuthentication.BuildToken(user.CreateClaims());
            var newRefresh = await _refresh.CreateRefreshTokenFor(user.Id, Guid.Parse(newJwt.Id));
            return Ok(new AuthResult(_jwtAuthentication.WriteToken(newJwt), newRefresh));
        }

        /// <summary>
        /// Revokes refresh token, related to current access token
        /// </summary>
        /// <response code="400">When access token is invalid</response>
        /// <response code="401">Not authorized</response>
        /// <response code="404">When active refresh token related to your access token not found</response>
        [HttpDelete("logout")]
        [Authorize]
        public Task<ActionResult> Logout()
        {
            if (User.FindFirstValue(JwtRegisteredClaimNames.Jti) is not string Jti
                || !Guid.TryParse(Jti, out var accessId))
                return Task.FromResult<ActionResult>(
                    Problem(statusCode: StatusCodes.Status400BadRequest, title: "Access token invalid"));

            return _refresh.RevokeByAccessToken(accessId).AsTask()
                .ContinueWith<ActionResult>(t => t.Result
                    ? NoContent()
                    : Problem(statusCode: StatusCodes.Status404NotFound,
                              title: "Related refresh token not found",
                              detail: "It seems like related refresh token has been alreary used or revoked"));
        }

        /// <summary>
        /// Revokes all refresh token, related to caller's user
        /// </summary>
        /// <response code="400">When access token is invalid</response>
        /// <response code="401">Not authorized</response>
        /// <response code="204">When deletion succeeded</response>
        [HttpDelete("logout/all")]
        [Authorize]
        public Task<ActionResult> LogoutAll()
        {
            if (!User.TryGetIdentifierAsGuid(out var userId))
                return Task.FromResult<ActionResult>(BadRequest());

            return _refresh.RevokeAll(userId).AsTask()
                .ContinueWith<ActionResult>(t => NoContent());
        }
    }
}
