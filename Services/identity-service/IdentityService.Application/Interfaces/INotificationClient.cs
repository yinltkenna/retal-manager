namespace IdentityService.Application.Interfaces
{
    public interface INotificationClient
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
