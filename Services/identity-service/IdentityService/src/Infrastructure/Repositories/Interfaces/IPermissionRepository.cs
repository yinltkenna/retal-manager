using IdentityService.src.Domain.Entities;

namespace IdentityService.src.Infrastructure.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission?> GetByIdAsync(Guid id);
        Task<List<Permission>> GetAllAsync();
        Task<List<Permission>> GetByGroupAsync(string group);

        // Get permissions by role
        Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
        Task<List<Permission>> GetPermissionsByRoleIdsAsync(List<Guid> roleIds);

        void Update(Permission permission);
    }
}
