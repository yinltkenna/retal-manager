using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories
{
    public class EmailVerificationTokenRepository(AppDbContext db) : IEmailVerificationTokenRepository
    {
        private readonly AppDbContext _db = db;

        public async Task AddAsync(EmailVerificationToken token)
        {
            await _db.EmailVerificationTokens.AddAsync(token);
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            var tokens = await _db.EmailVerificationTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();
            _db.EmailVerificationTokens.RemoveRange(tokens);
        }

        public async Task<EmailVerificationToken?> GetActiveByUserIdAsync(Guid userId)
        {
            return await _db.EmailVerificationTokens
                .Where(t => t.UserId == userId && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task<EmailVerificationToken?> GetByTokenAsync(string token)
        {
            return await _db.EmailVerificationTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public void Update(EmailVerificationToken token)
        {
            _db.EmailVerificationTokens.Update(token);
        }
    }
}
