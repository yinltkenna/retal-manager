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
        public async Task AddUserRolesRangeAsync(List<UserRole> userRoles)
        {
            await _db.UserRoles.AddRangeAsync(userRoles);
        }

        public async Task<int> AssignRolesToUsersBulkAsync(List<Guid> roleIds, List<Guid> userIds)
        {
            var existingPairs = await _db.UserRoles
                .Where(ur => userIds.Contains(ur.UserId) && roleIds.Contains(ur.RoleId))
                .Select(ur => new { ur.UserId, ur.RoleId })
                .ToListAsync();

            var newUserRoles = userIds
                .SelectMany(uId => roleIds.Select(rId => new { uId, rId }))
                .Where(pair => !existingPairs.Any(ex => ex.UserId == pair.uId && ex.RoleId == pair.rId))
                .Select(pair => new UserRole
                {
                    UserId = pair.uId,
                    RoleId = pair.rId
                })
                .ToList();

            if (newUserRoles.Count != 0)
            {
                await _db.UserRoles.AddRangeAsync(newUserRoles);
            }

            return newUserRoles.Count;
        }

        public async Task<bool> AssignRoleToUserAsync(Guid roleId, Guid userId)
        {
            var exists = await _db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (exists) return false;

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            await _db.UserRoles.AddAsync(userRole);
            return true;
        }

        public void Delete(Role role)
        {
            _db.Roles.Remove(role);
        }
        public async Task RemoveUserRolesRangeAsync(List<Guid> roleIds, List<Guid> userIds)
        {
            var toRemove = await _db.UserRoles
                .Where(ur => userIds.Contains(ur.UserId) && roleIds.Contains(ur.RoleId))
                .ToListAsync();

            if (toRemove.Count != 0)
            {
                _db.UserRoles.RemoveRange(toRemove);
            }
        }
        public Task<bool> ExistsRoleAsync(List<Guid> userId, List<Guid> roleId)
        {
            return _db.UserRoles.AnyAsync(ur => userId.Contains(ur.UserId) && roleId.Contains(ur.RoleId));
        }

        public Task<bool> ExistsRoleAsync(Guid userId, Guid roleId)
        {
            return _db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
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
        public async Task<List<UserRole>> GetExistingUserRolesAsync(List<Guid> userIds, List<Guid> roleIds)
        {
            return await _db.UserRoles
                .Where(ur => userIds.Contains(ur.UserId) && roleIds.Contains(ur.RoleId))
                .ToListAsync();
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

        public async Task<List<RolePermission>> GetExistingRolePermissionsAsync(List<Guid> roleIds, List<Guid> permissionIds)
        {
            return await _db.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId) && permissionIds.Contains(rp.PermissionId))
                .ToListAsync();
        }

        public async Task AddRolePermissionsRangeAsync(List<RolePermission> rolePermissions)
        {
            await _db.RolePermissions.AddRangeAsync(rolePermissions);
        }

        public async Task RemoveRolePermissionsRangeAsync(List<Guid> permissionIds, List<Guid> roleIds)
        {
            var toRemove = await _db.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId) && permissionIds.Contains(rp.PermissionId))
                .ToListAsync();

            if (toRemove.Count != 0)
            {
                _db.RolePermissions.RemoveRange(toRemove);
            }
        }
    }
}
