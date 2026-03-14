using PropertyService.Application.DTOs.Requests.Branch;
using PropertyService.Application.DTOs.Responses.Branch;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
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
