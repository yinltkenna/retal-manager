using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces
{
    public interface IEmailVerificationTokenRepository
    {
        Task AddAsync(EmailVerificationToken token);

        Task<EmailVerificationToken?> GetByTokenAsync(string token);

        Task<EmailVerificationToken?> GetActiveByUserIdAsync(Guid userId);

        void Update(EmailVerificationToken token);

        Task DeleteByUserIdAsync(Guid userId);
    }
}
