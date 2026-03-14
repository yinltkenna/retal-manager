namespace TenancyService.Application.DTOs.Requests.Tenant
{
    public class CreateTenantRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string? ProfileData { get; set; }
        public string? AvatarFileId { get; set; }
    }
}
