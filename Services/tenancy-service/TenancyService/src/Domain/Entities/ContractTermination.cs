namespace TenancyService.src.Domain.Entities
{
    public class ContractTermination : BaseEntity
    {
        public Guid ContractId { get; set; }
        public DateTime TerminationDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal RefundDeposit { get; set; }
    }
}
