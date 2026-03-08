using IdentityService.src.Application.DTOs.Requests.Role;
using IdentityService.src.Application.DTOs.Responses.Roles;
using IdentityService.src.Web.Common.TemplateResponses;

namespace IdentityService.src.Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<RoleDetailResponse?>> GetByIdAsync(Guid id);
        Task<ApiResponse<RoleDetailResponse?>> GetByNameAsync(string name);
        Task<ApiResponse<PagedResponse<RoleDetailResponse>>> GetAllAsync();
        Task<ApiResponse<RoleDetailResponse?>> CreateAsync(CreateRoleRequest request);
        Task<ApiResponse<RoleDetailResponse?>> UpdateAsync(Guid id, UpdateRoleRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id); // SoftDelete
        Task<ApiResponse<bool>> AssignRoleToUser(List<Guid> roleId, List<Guid> userIds); // Add many users to many roles
        Task<ApiResponse<bool>> AssignRoleToUser(Guid roleId, Guid userId); // Add one user to one role
        Task<ApiResponse<bool>> RemoveRoleFromUser(List<Guid> roleId, List<Guid> userIds); // Remove many users from many roles
        Task<ApiResponse<bool>> AssignPermissionsToRoles(List<Guid> permissionId, List<Guid> roleIds); // Add many permissions to many roles
        Task<ApiResponse<bool>> RemovePermissionFromRole(List<Guid> permissionId, List<Guid> roleIds); // Remove many permissions from many roles
        Task<ApiResponse<List<RoleDetailResponse>>> GetRolesByUserIdAsync(Guid userId);
    }
}
