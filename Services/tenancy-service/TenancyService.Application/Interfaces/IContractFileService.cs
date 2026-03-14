using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractFile;
using TenancyService.Application.DTOs.Responses.ContractFile;

namespace TenancyService.Application.Interfaces
{
    public interface IContractFileService
    {
        Task<ApiResponse<List<ContractFileResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractFileResponse>> AddAsync(AddContractFileRequest request);
        Task<ApiResponse<bool>> RemoveAsync(Guid id);
    }
}
