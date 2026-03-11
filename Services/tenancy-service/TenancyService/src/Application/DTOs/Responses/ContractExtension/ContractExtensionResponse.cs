namespace TenancyService.src.Application.DTOs.Responses.ContractExtension
{
    public class ContractExtensionResponse
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public DateTime OldEndDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
