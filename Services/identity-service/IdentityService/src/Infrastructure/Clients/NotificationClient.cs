using IdentityService.src.Application.Services.Interfaces;

namespace IdentityService.src.Infrastructure.Clients
{
    public class NotificationClient : INotificationClient
    {
        private readonly HttpClient _httpClient;
        public NotificationClient(
            HttpClient htppClient)
        {
            _httpClient = htppClient;
        }

        public Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            // stub - just pretend the notification went out successfully
            return Task.FromResult(true);
        }
    }
}