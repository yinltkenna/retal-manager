namespace IdentityService.src.Application.Services.Interfaces
{
    public interface ITenancyServiceClient
    {
        Task<Guid?> ClaimContractAsync(string codeContract, Guid userId);
    }
}
