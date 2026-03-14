using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractDepositTransaction;
using TenancyService.Application.DTOs.Responses.ContractDepositTransaction;

namespace TenancyService.Application.Interfaces
{
    public interface IContractDepositTransactionService
    {
        Task<ApiResponse<List<ContractDepositTransactionResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractDepositTransactionResponse>> CreateAsync(Guid contractId, CreateContractDepositTransactionRequest request);
    }
}
