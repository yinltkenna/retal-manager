using IdentityService.src.Domain.Entities;

namespace IdentityService.src.Application.Definitions
{
    public class RolePermissionDefinitions
    {
        public static IEnumerable<RolePermission> Get() =>
        [
            // Admin role permissions
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleAdminId,
                PermissionId = PermissionDefinitions.ViewId
            },
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleAdminId,
                PermissionId = PermissionDefinitions.CreateId
            },
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleAdminId,
                PermissionId = PermissionDefinitions.UpdateId
            },
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleAdminId,
                PermissionId = PermissionDefinitions.DeleteId
            },
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleAdminId,
                PermissionId = PermissionDefinitions.LockId
            },
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleAdminId,
                PermissionId = PermissionDefinitions.ViewLogsId
            },
            // User role permissions
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleUserId,
                PermissionId = PermissionDefinitions.ViewId
            },
            new RolePermission
            {
                RoleId = RoleDefinitions.RoleUserId,
                PermissionId = PermissionDefinitions.ViewLogsId
            }

        ];
    }
}
