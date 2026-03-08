using NotificationService.src.Application.Services.Interfaces;
using NotificationService.src.Infrastructure.Templates;

namespace NotificationService.src.Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailTemplate _emailTemplate;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
            _emailTemplate = new EmailTemplate(logger);
        }

        public async Task SendEmailVerificationAsync(string email, string verificationToken, string phoneNumber)
        {
            try
            {
                _logger.LogInformation("=== SENDING EMAIL VERIFICATION ===");
                _logger.LogInformation("Recipient: {Email}", email);
                
                // Generate and log email template
                _emailTemplate.LogEmailVerificationTemplate(email, verificationToken, phoneNumber);

                _logger.LogInformation("=== EMAIL LOGGED SUCCESSFULLY ===\n");

                // Simulate async email sending
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email verification to {Email}", email);
                throw;
            }
        }
    }
}
