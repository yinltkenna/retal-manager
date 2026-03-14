namespace TenancyService.Application.DTOs.Responses.Tenant
{
    public class TenantResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string? ProfileData { get; set; }
        public string? AvatarFileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
