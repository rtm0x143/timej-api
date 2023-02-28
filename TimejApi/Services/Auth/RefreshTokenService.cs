using Microsoft.EntityFrameworkCore;
using TimejApi.Data;
using TimejApi.Data.Entities;

namespace TimejApi.Services.Auth
{
    public class RefreshTokenService : IRefreshTokenService
    {
        public static TimeSpan FallbackRefreshLifeTime { get; set; } = TimeSpan.FromHours(12);

        private readonly IConfiguration _config;
        private readonly ScheduleDbContext _context;
        private readonly ILogger<RefreshTokenService> _logger;

        public RefreshTokenService(IConfiguration config, ILogger<RefreshTokenService> logger, ScheduleDbContext context)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }

        public ValueTask<string> CreateRefreshTokenFor(Guid userId)
        {
            if (_config["RefreshToken:LifeTime"] is not string lifeTimeString
               || !TimeSpan.TryParse(lifeTimeString, out var lifeTime))
            {
                lifeTime = FallbackRefreshLifeTime;
                _logger?.LogWarning("Configuration doesn't contain 'RefreshToken:LifeTime' prop or it is invalid");
            }

            var auth = new AuthenticationModel()
            {
                RefreshTokenId = Guid.NewGuid(),
                RefreshTokenExpiration = DateTime.UtcNow + lifeTime,
                UserId = userId
            };

            _context.AuthenticationModels.Add(auth);
            
            // In future we can encode there something more than just id
            return new(_context.SaveChangesAsync()
                .ContinueWith((_) => Convert.ToBase64String(auth.RefreshTokenId.ToByteArray())));
        }

        protected async ValueTask<bool> _validateToken(Guid tokenId)
        {
            var model = await _context.AuthenticationModels.FindAsync(tokenId);
            if (model == null) return false;
            _context.Entry(model).State = EntityState.Detached;
            return model.RefreshTokenExpiration > DateTime.UtcNow;
        }

        protected ValueTask _revoke(Guid tokenId)
        {
            _context.AuthenticationModels.Remove(new AuthenticationModel { RefreshTokenId = tokenId });
            return new(_context.SaveChangesAsync());
        }

        public async ValueTask<bool> ValidateToken(string refreshToken) =>
            await _validateToken(new Guid(Convert.FromBase64String(refreshToken)));

        public ValueTask Revoke(string refreshToken) => _revoke(new Guid(Convert.FromBase64String(refreshToken)));

        public async ValueTask<bool> UseRefreshToken(string refreshToken)
        {
            var tokenId = new Guid(Convert.FromBase64String(refreshToken));
            if (!await _validateToken(tokenId)) return false;
            await _revoke(tokenId);
            return true;
        }

        public ValueTask RevokeAll(Guid userId) => new(
            _context.AuthenticationModels.Where(m => m.UserId == userId).ExecuteDeleteAsync());
    }
}
