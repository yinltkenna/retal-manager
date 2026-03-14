namespace TenancyService.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
        public string PermanentAddress { get; set; } = string.Empty;
        public string? ProfileData { get; set; }
        public string? AvatarFileId { get; set; }
    }
}
