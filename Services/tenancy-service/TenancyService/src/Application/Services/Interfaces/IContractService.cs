using TenancyService.src.Application.DTOs.Requests.Contract;
using TenancyService.src.Application.DTOs.Responses.Contract;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
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
