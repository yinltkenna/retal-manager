using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractTermination;
using TenancyService.Application.DTOs.Responses.ContractTermination;

namespace TenancyService.Application.Interfaces
{
    public interface IContractTerminationService
    {
        Task<ApiResponse<List<ContractTerminationResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractTerminationResponse>> CreateAsync(Guid contractId, CreateContractTerminationRequest request);
    }
}
