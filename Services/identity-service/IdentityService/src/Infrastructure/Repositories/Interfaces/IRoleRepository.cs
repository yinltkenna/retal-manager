using IdentityService.src.Domain.Entities;

namespace IdentityService.src.Infrastructure.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string name);
        Task<List<Role>> GetAllAsync();

        Task AddAsync(Role role);
        void Update(Role role);
        void Delete(Role role);

        // Get Role by user id
        Task<List<Role>> GetRolesByUserIdAsync(Guid userId);
        Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId);
    }
}
