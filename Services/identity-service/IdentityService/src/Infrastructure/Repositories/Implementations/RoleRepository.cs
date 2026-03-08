using IdentityService.src.Domain.Entities;
using IdentityService.src.Infrastructure.Data;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.src.Infrastructure.Repositories.Implementations
{
    public class RoleRepository(AppDbContext db) : IRoleRepository
    {
        private readonly AppDbContext _db = db;

        public async Task AddAsync(Role role)
        {
            await _db.Roles.AddAsync(role);
        }

        public void Delete(Role role)
        {
            _db.Roles.Remove(role);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(Guid id)
        {
            return await _db.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _db.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            return await _db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Join(_db.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .ToListAsync();
        }

        public Task<List<Role>> GetRolesByUserIdAsync(Guid userId)
        {
            return _db.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_db.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                .ToListAsync();
        }

        public void Update(Role role)
        {
            _db.Roles.Update(role);
        }
    }
}
