namespace TenancyService.src.Application.DTOs.Responses.ContractMember
{
    public class ContractMemberResponse
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid TenantId { get; set; }
        public bool IsRepresentative { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
