namespace TenancyService.Domain.Entities
{
    public class ContractFile : BaseEntity
    {
        public Guid ContractId { get; set; }
        public string FileId { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
