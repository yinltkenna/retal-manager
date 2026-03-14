namespace TenancyService.Domain.Entities
{
    public class ContractDepositTransaction : BaseEntity
    {
        public Guid ContractId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
    }
}
