using TenancyService.src.Application.DTOs.Requests.ContractFile;
using TenancyService.src.Application.DTOs.Responses.ContractFile;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
{
    public interface IContractFileService
    {
        Task<ApiResponse<List<ContractFileResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractFileResponse>> AddAsync(AddContractFileRequest request);
        Task<ApiResponse<bool>> RemoveAsync(Guid id);
    }
}
