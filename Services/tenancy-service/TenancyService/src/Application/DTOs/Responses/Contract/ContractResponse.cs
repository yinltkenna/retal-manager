namespace TenancyService.src.Application.DTOs.Responses.Contract
{
    public class ContractResponse
    {
        public Guid Id { get; set; }
        public string ContractCode { get; set; } = string.Empty;
        public Guid RoomId { get; set; }
        public Guid OwnerTenantId { get; set; }
        public string SignedRoomNumber { get; set; } = string.Empty;
        public decimal SignedPrice { get; set; }
        public string BranchAddress { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ActualPrice { get; set; }
        public string SignedRoomType { get; set; } = string.Empty;
        public decimal DepositAmount { get; set; }
        public string PaymentCycle { get; set; } = string.Empty;
        public int BillingDay { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
