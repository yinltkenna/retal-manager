namespace TenancyService.src.Application.DTOs.Responses.ContractTermination
{
    public class ContractTerminationResponse
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public DateTime TerminationDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal RefundDeposit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
