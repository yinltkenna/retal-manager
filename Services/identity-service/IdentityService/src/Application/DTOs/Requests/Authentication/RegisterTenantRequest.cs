namespace IdentityService.src.Application.DTOs.Requests.Authentication
{
    public class RegisterTenantRequest
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public string? CodeContract { get; set; }
    }
}
