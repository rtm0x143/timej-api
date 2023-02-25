using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace TimejApi.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGatSubAsGuid(this ClaimsPrincipal claims, out Guid sub)
        {
            sub = Guid.Empty;
            return claims.FindFirst(JwtRegisteredClaimNames.Sub)?.Value is string subString
                && Guid.TryParse(subString, out sub);
        }
    }
}
