using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractExtension;
using TenancyService.Application.DTOs.Responses.ContractExtension;

namespace TenancyService.Application.Interfaces
{
    public interface IContractExtensionService
    {
        Task<ApiResponse<List<ContractExtensionResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractExtensionResponse>> CreateAsync(Guid contractId, CreateContractExtensionRequest request);
    }
}
