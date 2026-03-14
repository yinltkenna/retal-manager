using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
    {
        private readonly AppDbContext _db = db;

        public async Task AddAsync(RefreshToken token)
        {
            await _db.RefreshTokens.AddAsync(token);
        }

        public void Delete(RefreshToken token)
        {
            _db.RefreshTokens.Remove(token);
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            var tokens = await _db.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            _db.RefreshTokens.RemoveRange(tokens);
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var expiredTokens = await _db.RefreshTokens
                .Where(t => t.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();
            _db.RefreshTokens.RemoveRange(expiredTokens);
        }

        public async Task<RefreshToken?> GetActiveByUserIdAsync(Guid userId)
        {
            return await _db.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task<RefreshToken?> GetByIdAsync(Guid id)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<RefreshToken?> GetByRawTokenAsync(string rawToken)
        {
            // since stored tokens are hashed with bcrypt, we cannot query directly
            var allTokens = await _db.RefreshTokens.ToListAsync();
            foreach (var t in allTokens)
            {
                if (BCrypt.Net.BCrypt.Verify(rawToken, t.Token))
                    return t;
            }
            return null;
        }
        public async Task<List<RefreshToken>> GetByUserIdAsync(Guid userId)
        {
            return await _db.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public void Update(RefreshToken token)
        {
            _db.RefreshTokens.Update(token);
        }
    }
}
