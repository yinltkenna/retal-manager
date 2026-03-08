using IdentityService.src.Domain.Entities;

namespace IdentityService.src.Infrastructure.Repositories.Interfaces
{

    public interface IRefreshTokenRepository
    {
        // Create
        Task AddAsync(RefreshToken token);

        // Read
        Task<RefreshToken?> GetByIdAsync(Guid id);
        Task<RefreshToken?> GetByTokenAsync(string token); // token đã hash
        // verify against raw token string using bcrypt
        Task<RefreshToken?> GetByRawTokenAsync(string rawToken);
        Task<List<RefreshToken>> GetByUserIdAsync(Guid userId);
        Task<RefreshToken?> GetActiveByUserIdAsync(Guid userId);

        // Update
        void Update(RefreshToken token);

        // Delete / Cleanup
        void Delete(RefreshToken token);
        Task DeleteByUserIdAsync(Guid userId);
        Task DeleteExpiredTokensAsync();
    }
}
