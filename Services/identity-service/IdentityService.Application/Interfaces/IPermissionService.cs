using IdentityService.Application.Common.TemplateResponses;
using IdentityService.Application.DTOs.Requests.Permession;
using IdentityService.Application.DTOs.Responses.Permessions;

namespace IdentityService.Application.Interfaces
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
