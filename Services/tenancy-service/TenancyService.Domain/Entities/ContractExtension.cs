namespace TenancyService.Domain.Entities
{
    public class ContractExtension : BaseEntity
    {
        public Guid ContractId { get; set; }
        public DateTime OldEndDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
