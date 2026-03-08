namespace IdentityService.src.Application.DTOs.Requests.User
{
    public class UpdateUserRequest
    {
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
    }
}
