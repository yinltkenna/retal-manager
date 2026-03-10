using IdentityService.src.Domain.Entities;
using IdentityService.src.Infrastructure.Data;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.src.Infrastructure.Repositories.Implementations
{
    public class PermissionRepository(AppDbContext db) : IPermissionRepository
    {
        private readonly AppDbContext _db = db;

        public async Task AddAsync(Permission permission)
        {
            await _db.Permissions.AddAsync(permission);
        }

        public async Task AddRolePermissionsRangeAsync(List<RolePermission> rolePermissions)
        {
            await _db.RolePermissions.AddRangeAsync(rolePermissions);
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _db.Permissions.ToListAsync();
        }

        public async Task<List<Permission>> GetByGroupAsync(string group)
        {
            return await _db.Permissions
                .Where(p => p.Group == group)
                .ToListAsync();
        }

        public async Task<Permission?> GetByIdAsync(Guid id)
        {
            return await _db.Permissions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<List<RolePermission>> GetExistingRolePermissionsAsync(List<Guid> roleIds, List<Guid> permissionIds)
        {
            return _db.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId) && permissionIds.Contains(rp.PermissionId))
                .ToListAsync();
        }

        public Task<Permission?> GetPermissionByCodeAsync(string code)
        {
            return _db.Permissions.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(Guid roleId)
        {
            return await _db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Join(_db.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdsAsync(List<Guid> roleIds)
        {
            return await _db.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Join(_db.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .Distinct()
                .ToListAsync();
        }

        public Task RemoveRolePermissionsRangeAsync(List<Guid> roleIds, List<Guid> permissionIds)
        {
            throw new NotImplementedException();
        }

        public void Update(Permission permission)
        {
            _db.Permissions.Update(permission);
        }
    }
}
