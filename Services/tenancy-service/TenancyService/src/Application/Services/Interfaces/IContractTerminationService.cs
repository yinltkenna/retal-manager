using TenancyService.src.Application.DTOs.Requests.ContractTermination;
using TenancyService.src.Application.DTOs.Responses.ContractTermination;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
{
    public interface IContractTerminationService
    {
        Task<ApiResponse<List<ContractTerminationResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractTerminationResponse>> CreateAsync(Guid contractId, CreateContractTerminationRequest request);
    }
}
