using TenancyService.src.Application.DTOs.Requests.ContractDepositTransaction;
using TenancyService.src.Application.DTOs.Responses.ContractDepositTransaction;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
{
    public interface IContractDepositTransactionService
    {
        Task<ApiResponse<List<ContractDepositTransactionResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractDepositTransactionResponse>> CreateAsync(Guid contractId, CreateContractDepositTransactionRequest request);
    }
}
