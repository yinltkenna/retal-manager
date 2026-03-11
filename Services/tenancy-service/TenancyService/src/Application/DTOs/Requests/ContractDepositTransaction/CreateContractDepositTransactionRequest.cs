namespace TenancyService.src.Application.DTOs.Requests.ContractDepositTransaction
{
    public class CreateContractDepositTransactionRequest
    {
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
    }
}
