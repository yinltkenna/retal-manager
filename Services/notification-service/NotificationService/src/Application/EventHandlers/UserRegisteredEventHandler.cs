using EventContracts.Identity;
using NotificationService.src.Application.Services.Interfaces;

namespace NotificationService.src.Application.EventHandlers
{
    public class UserRegisteredEventHandler
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<UserRegisteredEventHandler> _logger;

        public UserRegisteredEventHandler(
            IEmailService emailService,
            ILogger<UserRegisteredEventHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task HandleAsync(UserRegisteredEvent @event)
        {
            try
            {
                _logger.LogInformation("📨 Received UserRegisteredEvent for user {UserId}", @event.UserId);
                _logger.LogInformation("   Email: {Email}", @event.Email);
                _logger.LogInformation("   Phone: {PhoneNumber}", @event.PhoneNumber);

                // Send verification email
                await _emailService.SendEmailVerificationAsync(
                    @event.Email,
                    @event.VerificationToken,
                    @event.PhoneNumber);

                _logger.LogInformation("✅ Email verification sent successfully for user {UserId}", @event.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error handling UserRegisteredEvent for user {UserId}", @event.UserId);
                throw;
            }
        }
    }
}
