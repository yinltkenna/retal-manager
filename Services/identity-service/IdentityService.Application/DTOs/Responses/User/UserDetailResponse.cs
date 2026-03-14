using IdentityService.Application.Common.Helper;
using IdentityService.Application.DTOs.Responses.Permessions;
using System.Text.Json.Serialization;

namespace IdentityService.Application.DTOs.Responses.User
{
    public class UserDetailResponse
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string? Username { get; set; }
        //public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        [JsonConverter(typeof(VietnamDateTimeConverter))]
        public DateTime? LastPasswordChangedAt { get; set; }
        [JsonConverter(typeof(VietnamDateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonConverter(typeof(VietnamDateTimeConverter))]
        public DateTime? LastUpdatedAt { get; set; }
        [JsonConverter(typeof(VietnamDateTimeConverter))]
        public DateTime? LockOutEnd { get; set; }
        public List<string>? Roles { get; set; }
        public List<PermissionResponse>? Permissions { get; set; }
    }
}
