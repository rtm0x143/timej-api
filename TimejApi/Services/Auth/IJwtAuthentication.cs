using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TimejApi.Services.Auth
{
    public interface IJwtAuthentication
    {
        public JwtSecurityToken BuildToken(IEnumerable<Claim> claims);
        public string WriteToken(JwtSecurityToken token);
        public TokenValidationParameters CreateValidationParameters();
        public bool ValidateToken(string token, [NotNullWhen(true)] out ClaimsPrincipal? claims);

        public const string RoleClaimType = "role";
    }
}
