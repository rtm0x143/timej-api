using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimejApi.Services.Auth;
using UserEntity = TimejApi.Data.Entities.User;

namespace TimejApi.Services.User
{
    public static class UserEntityExtensions
    {
        public static List<Claim> CreateClaims(this UserEntity user)
        {
            var claims = new List<Claim>() { new(JwtRegisteredClaimNames.Sub, user.Id.ToString()) };
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                    claims.Add(new Claim(IJwtAuthentication.RoleClaimType, Enum.GetName(role.Role)!));
            }

            return claims;
        }
    }
}
