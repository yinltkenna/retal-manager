using PropertyService.src.Application.DTOs.Requests.Branch;
using PropertyService.src.Application.DTOs.Responses.Branch;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IBranchService
    {
        Task<ApiResponse<BranchResponse>> CreateAsync(CreateBranchRequest request);
        Task<ApiResponse<BranchResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<List<BranchResponse>>> GetAllAsync();
        Task<ApiResponse<BranchResponse>> UpdateAsync(Guid id, UpdateBranchRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
