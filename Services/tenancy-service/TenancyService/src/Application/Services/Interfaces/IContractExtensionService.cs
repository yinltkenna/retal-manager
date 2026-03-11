using TenancyService.src.Application.DTOs.Requests.ContractExtension;
using TenancyService.src.Application.DTOs.Responses.ContractExtension;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
{
    public interface IContractExtensionService
    {
        Task<ApiResponse<List<ContractExtensionResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractExtensionResponse>> CreateAsync(Guid contractId, CreateContractExtensionRequest request);
    }
}
