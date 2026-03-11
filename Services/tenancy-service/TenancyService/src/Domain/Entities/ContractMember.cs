namespace TenancyService.src.Domain.Entities
{
    public class ContractMember : BaseEntity
    {
        public Guid ContractId { get; set; }
        public Guid TenantId { get; set; }
        public bool IsRepresentative { get; set; }
    }
}
