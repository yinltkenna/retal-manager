using IdentityService.Application.Common.TemplateResponses;
using IdentityService.Application.DTOs.Requests.Role;
using IdentityService.Application.DTOs.Responses.Roles;

namespace IdentityService.Application.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<RoleDetailResponse?>> GetByIdAsync(Guid id);
        Task<ApiResponse<RoleDetailResponse?>> GetByNameAsync(string name);
        Task<ApiResponse<PagedResponse<RoleDetailResponse>>> GetAllAsync();
        Task<ApiResponse<RoleDetailResponse?>> CreateAsync(CreateRoleRequest request);
        Task<ApiResponse<RoleDetailResponse?>> UpdateAsync(Guid id, UpdateRoleRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id); // SoftDelete

        // This function is for adding many users to many roles, it can be used in the UI when we want to assign roles to users in bulk, and we don't want to use the single assign function.
        Task<ApiResponse<bool>> AssignRoleToUser(List<Guid> roleId, List<Guid> userIdsBeAssign, Guid userIdsRequest); // Add many users to many roles

        // This function is for adding one user to one role, it can be used in the UI when we want to assign a role to a user, and we don't want to use the bulk assign function.
        Task<ApiResponse<bool>> AssignRoleToUser(Guid roleId, Guid userIdBeAssign, Guid userIdRequest); // Add one user to one role
        Task<ApiResponse<bool>> RemoveRoleFromUser(List<Guid> roleId, List<Guid> userIdsBeRemove, Guid userIdRequest); // Remove many users from many roles
        Task<ApiResponse<bool>> AssignPermissionsToRoles(List<Guid> permissionId, List<Guid> roleIds); // Add many permissions to many roles
        Task<ApiResponse<bool>> RemovePermissionFromRole(List<Guid> permissionId, List<Guid> roleIds); // Remove many permissions from many roles
        Task<ApiResponse<List<RoleDetailResponse>>> GetRolesByUserIdAsync(Guid userId);
    }
}
