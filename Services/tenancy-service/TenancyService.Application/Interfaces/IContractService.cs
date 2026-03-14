using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.Contract;
using TenancyService.Application.DTOs.Responses.Contract;

namespace TenancyService.Application.Interfaces
{
    public interface IContractService
    {
        Task<ApiResponse<List<ContractResponse>>> GetAllAsync();
        Task<ApiResponse<ContractResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<ContractResponse>> CreateAsync(CreateContractRequest request);
        Task<ApiResponse<ContractResponse>> UpdateAsync(Guid id, UpdateContractRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
