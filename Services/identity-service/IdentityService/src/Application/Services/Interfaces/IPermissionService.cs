using IdentityService.src.Application.DTOs.Requests.Permession;
using IdentityService.src.Application.DTOs.Responses.Permessions;
using IdentityService.src.Web.Common.TemplateResponses;

namespace IdentityService.src.Application.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<ApiResponse<List<PermissionResponse>>> GetAllAsync();
        Task<ApiResponse<PermissionResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<string>> CreateAsync(CreatePermissionRequest request);
        Task<ApiResponse<string>> UpdateAsync(UpdatePermissionRequest request);
        Task<ApiResponse<string>> AssignToRoleAsync(AssignPermissionsToRoleRequest request);
        Task<ApiResponse<string>> RemoveFromRoleAsync(RemovePermissionsToRoleRequest request);
    }
}
