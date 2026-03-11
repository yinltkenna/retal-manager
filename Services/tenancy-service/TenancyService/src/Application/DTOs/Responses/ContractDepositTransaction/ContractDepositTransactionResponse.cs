namespace TenancyService.src.Application.DTOs.Responses.ContractDepositTransaction
{
    public class ContractDepositTransactionResponse
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
