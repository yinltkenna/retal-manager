namespace NotificationService.src.Application.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email verification template (mock - logs to console)
        /// </summary>
        Task SendEmailVerificationAsync(string email, string verificationToken, string phoneNumber);
    }
}
