using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission?> GetByIdAsync(Guid id);
        Task<List<Permission>> GetAllAsync();
        Task<List<Permission>> GetByGroupAsync(string group);
        Task<List<RolePermission>> GetExistingRolePermissionsAsync(List<Guid> roleIds, List<Guid> permissionIds);
        Task AddRolePermissionsRangeAsync(List<RolePermission> rolePermissions);
        Task RemoveRolePermissionsRangeAsync(List<Guid> roleIds, List<Guid> permissionIds);
        Task<Permission?> GetPermissionByCodeAsync(string code);
        // Get permissions by role
        Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
        Task<List<Permission>> GetPermissionsByRoleIdsAsync(List<Guid> roleIds);
        Task AddAsync(Permission permission);
        void Update(Permission permission);
    }
}
