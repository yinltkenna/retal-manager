namespace TenancyService.Application.DTOs.Responses.ContractFile
{
    public class ContractFileResponse
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public string FileId { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
