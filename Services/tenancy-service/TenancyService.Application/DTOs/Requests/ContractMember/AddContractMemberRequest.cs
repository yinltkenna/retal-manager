namespace TenancyService.Application.DTOs.Requests.ContractMember
{
    public class AddContractMemberRequest
    {
        public Guid ContractId { get; set; }
        public Guid TenantId { get; set; }
        public bool IsRepresentative { get; set; }
    }
}
