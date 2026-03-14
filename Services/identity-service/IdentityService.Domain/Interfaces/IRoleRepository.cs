using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string name);
        Task<List<Role>> GetAllAsync();

        // Check exits role with userid with roleid
        Task<bool> ExistsRoleAsync(List<Guid> userId, List<Guid> roleId);
        Task<bool> ExistsRoleAsync(Guid userId, Guid roleId);

        // Operations
        Task AddAsync(Role role);
        Task AddUserRolesRangeAsync(List<UserRole> userRoles);
        void Update(Role role);
        void Delete(Role role);
        Task RemoveUserRolesRangeAsync(List<Guid> roleIds, List<Guid> userIds);
        // Get Role by user id
        Task<List<UserRole>> GetExistingUserRolesAsync(List<Guid> userIds, List<Guid> roleIds);
        Task <List<Role>> GetRolesByUserIdAsync(Guid userId);
        Task <List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);

        // Assign
        Task <int> AssignRolesToUsersBulkAsync(List<Guid> roleId, List<Guid> userIdsBeAssign);
        Task <bool> AssignRoleToUserAsync(Guid roleId, Guid userIdsBeAssign);


        Task<List<RolePermission>> GetExistingRolePermissionsAsync(List<Guid> roleIds, List<Guid> permissionIds);
        Task AddRolePermissionsRangeAsync(List<RolePermission> rolePermissions);
        Task RemoveRolePermissionsRangeAsync(List<Guid> permissionIds, List<Guid> roleIds);

    }
}
