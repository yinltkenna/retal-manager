namespace NotificationService.src.Infrastructure.Templates
{
    public class EmailTemplate
    {
        private readonly ILogger _logger;

        public EmailTemplate(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Generate and log email verification template
        /// </summary>
        public void LogEmailVerificationTemplate(string email, string verificationToken, string phoneNumber)
        {
            // Get last 4 digits of phone
            string last4Digits = phoneNumber.Length >= 4 ? phoneNumber.Substring(phoneNumber.Length - 4) : phoneNumber;

            string frontendBaseUrl = "http://localhost:3000"; // Frontend URL (update based on your setup)
            string verificationLink = $"{frontendBaseUrl}/verify-email?userId=YOUR_USER_ID&token={verificationToken}";

            string emailBody = $@"
╔══════════════════════════════════════════════════════════════╗
║                  EMAIL VERIFICATION TEMPLATE                 ║
╚══════════════════════════════════════════════════════════════╝

TO: {email}
SUBJECT: Email Verification Required

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Hello,

Thank you for registering with our Rental Management system!

To complete your registration and verify your email address, please click the link below:

🔗 VERIFICATION LINK:
{verificationLink}

📱 SECURITY NOTE:
When you click the link above, you will be asked to enter the last 4 digits of your phone number:
Last 4 digits: {last4Digits}

⏰ TOKEN EXPIRY:
This link will expire in 10 minutes.

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

TOKEN INFO (for testing/debugging):
- Email: {email}
- Verification Token: {verificationToken}
- Phone Last 4 Digits: {last4Digits}
- Token Expires At: {DateTime.UtcNow.AddMinutes(10):yyyy-MM-dd HH:mm:ss} UTC

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

If you did not register for this account, please ignore this email.

Best regards,
Rental Management Team
";

            _logger.LogInformation(emailBody);
        }
    }
}
