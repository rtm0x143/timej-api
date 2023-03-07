using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using TimejApi.Data.Entities;

namespace TimejApi.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetIdentifierAsGuid(this ClaimsPrincipal claims, out Guid sub)
        {
            sub = Guid.Empty;
            return claims.FindFirst(ClaimTypes.NameIdentifier)?.Value is string subString
                && Guid.TryParse(subString, out sub);
        }

        public static bool IdentifierEqualsOrInRole(this ClaimsPrincipal claims, in Guid equalTo, string role) =>
            claims.TryGetIdentifierAsGuid(out var callerId) && (callerId == equalTo || claims.IsInRole(role));
    }
}
