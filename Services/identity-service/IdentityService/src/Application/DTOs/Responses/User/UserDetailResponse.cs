using IdentityService.src.Application.DTOs.Responses.Permessions;

namespace IdentityService.src.Application.DTOs.Responses.User
{
    public class UserDetailResponse
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public DateTime? LastPasswordChangedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? LockOutEnd { get; set; }
        public List<string>? Roles { get; set; }
        public List<PermissionResponse>? Permissions { get; set; }
    }
}
