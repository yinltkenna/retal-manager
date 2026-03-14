namespace IdentityService.Application.DTOs.Requests.Authentication
{
    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
    }
}
