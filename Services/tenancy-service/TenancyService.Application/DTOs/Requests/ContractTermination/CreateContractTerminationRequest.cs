namespace TenancyService.Application.DTOs.Requests.ContractTermination
{
    public class CreateContractTerminationRequest
    {
        public DateTime TerminationDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal RefundDeposit { get; set; }
    }
}
