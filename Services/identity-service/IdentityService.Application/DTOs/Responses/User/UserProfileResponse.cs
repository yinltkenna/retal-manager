namespace IdentityService.Application.DTOs.Responses.User
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
