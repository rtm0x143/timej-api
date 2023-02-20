using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace TimejApi.Services.Auth
{
    public interface IJwtAuthentication
    {
        public string BuildToken(IEnumerable<Claim> claims);
        public TokenValidationParameters CreateValidationParameters();
        public bool ValidateToken(string token, [NotNullWhen(true)] out ClaimsPrincipal? claims);
    }
}
