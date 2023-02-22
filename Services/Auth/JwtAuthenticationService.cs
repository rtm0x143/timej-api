using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TimejApi.Services.Auth
{
    /// <summary>
    /// Builds and validates token using given Configuration for resolving options
    /// </summary>
    public class JwtAuthenticationService : IJwtAuthentication
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtAuthenticationService>? _logger;

        public static TimeSpan FallbackTokenLifeTime { get; set; } = TimeSpan.FromHours(1);

        public JwtAuthenticationService(IConfiguration config, ILogger<JwtAuthenticationService>? logger)
        {
            _config = config;
            _logger = logger;
        }

        private void _extractConfigProps(out string appId, out SecurityKey signKey)
        {
            appId = _config["ApplicationId"]
                ?? throw new ArgumentNullException("Configuration's prop 'Jwt:ApplicationId' was Null");
            signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["Jwt:SigningKey"]
                ?? throw new ArgumentNullException("Configuration's prop 'Jwt:SigningKey' was Null")));
        }

        private void _extractConfigProps(out string appId, out SecurityKey signKey, out TimeSpan lifeTime)
        {
            if (_config["Jwt:LifeTime"] is not string lifeTimeStr ||
                !TimeSpan.TryParse(lifeTimeStr, out lifeTime))
            {
                lifeTime = FallbackTokenLifeTime;
                _logger?.LogWarning("Configuration doesn't contain 'Jwt:LifeTime' prop or it is invalid");
            }

            _extractConfigProps(out appId, out signKey);
        }

        /// <exception cref="ArgumentNullException">When Configuration is invalid</exception>
        public string BuildToken(IEnumerable<Claim> claims)
        {
            _extractConfigProps(out var appId, out var signKey, out var lifeTime);

            var token = new JwtSecurityToken(
                issuer: appId,
                audience: appId,
                claims: claims,
                expires: DateTime.UtcNow.Add(lifeTime),
                signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenValidationParameters CreateValidationParameters()
        {
            _extractConfigProps(out var appId, out var signKey);

            return new()
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signKey,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
                ValidateIssuer = true,
                ValidIssuers = (_config["Jwt:Issuers"]?.Split(";") ?? Array.Empty<string>()).Append(appId),
                ValidateAudience = true,
                ValidAudiences = (_config["Jwt:Audiences"]?.Split(";") ?? Array.Empty<string>()).Append(appId)
            };
        }
        
        public bool ValidateToken(string token, [NotNullWhen(true)] out ClaimsPrincipal? claims)
        {
            try
            {
                claims = new JwtSecurityTokenHandler().ValidateToken(token, CreateValidationParameters(), out var validToken);
            }
            catch (Exception e)
            {
                _logger?.LogInformation(e, "Token ({}) validation failed", token);
                claims = null;
            }

            return claims != null;
        }
    }
}
