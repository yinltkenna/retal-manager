namespace IdentityService.Application.Interfaces
{
    public interface ITenancyServiceClient
    {
        Task<Guid?> ClaimContractAsync(string codeContract, Guid userId);
    }
}
