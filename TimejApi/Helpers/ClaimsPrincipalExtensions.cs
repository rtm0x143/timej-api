using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using TimejApi.Data.Entities;

namespace TimejApi.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetSubAsGuid(this ClaimsPrincipal claims, out Guid sub)
        {
            sub = Guid.Empty;
            return claims.FindFirst(JwtRegisteredClaimNames.Sub)?.Value is string subString
                && Guid.TryParse(subString, out sub);
        }

        public static bool SubEqualsOrInRole(this ClaimsPrincipal claims, in Guid equalTo, string role) =>
            claims.TryGetSubAsGuid(out var callerId) && (callerId == equalTo || !claims.IsInRole(role));
    }
}
