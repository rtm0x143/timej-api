using System.IdentityModel.Tokens.Jwt;

namespace TimejApi.Services.Auth
{
    public interface IRefreshTokenService
    {
        /// <returns>True if refresh succeded</returns>
        public ValueTask<bool> ValidateToken(string refreshToken);
        /// <summary>
        /// Unvalidates refresh token 
        /// </summary>
        public ValueTask Revoke(string refreshToken);
        /// <summary>
        /// Validates token and revokes it if succeeded
        /// </summary>
        /// <returns>True if refresh token was valid</returns>
        public ValueTask<bool> UseRefreshToken(string refreshToken);
        /// <summary>
        /// Unvalidates all user's refresh token 
        /// </summary>
        public ValueTask RevokeAll(Guid userId);

        public ValueTask<string> CreateRefreshTokenFor(Guid userId);
    }
}
