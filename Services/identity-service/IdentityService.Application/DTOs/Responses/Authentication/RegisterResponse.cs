namespace IdentityService.Application.DTOs.Responses.Authentication
{
    public class RegisterResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string IsEmailConfirmed { get; set; }
    }
}
