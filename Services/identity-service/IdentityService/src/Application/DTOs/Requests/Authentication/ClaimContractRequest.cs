namespace IdentityService.src.Application.DTOs.Requests.Authentication
{
    public class ClaimContractRequest
    {
        public string CodeContract { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
