namespace TenancyService.Application.DTOs.Responses.TenantIdentityDocument
{
    public class TenantIdentityDocumentResponse
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
        public string IssuedPlace { get; set; } = string.Empty;
        public string? ImageFrontFileId { get; set; }
        public string? ImageBackFileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
