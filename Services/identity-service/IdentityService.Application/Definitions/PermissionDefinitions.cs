using EventContracts.Authorization;
using EventContracts.Authorization.Permissions.IdentityService;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Definitions
{
    public static class PermissionDefinitions
    {
        public static readonly Guid UserViewId = DefId.UserViewId;
        public static readonly Guid UserCreateId = DefId.UserCreateId;
        public static readonly Guid UserUpdateId = DefId.UserUpdateId;
        public static readonly Guid UserDeleteId = DefId.UserDeleteId;
        public static readonly Guid UserLockId = DefId.UserLockId;
        public static readonly Guid UserViewLogsId = DefId.UserViewLogsId;

        public static IEnumerable<Permission> Get() =>
        [
            new Permission
            {
                Id = UserViewId,
                Code = UserPermissions.VIEW,
                Name = "View User",
                Group = "User",
                Type = "API",
                SortOrder = 1,
                IsActive = true
            },
            new Permission
            {
                Id = UserCreateId,
                Code = UserPermissions.CREATE,
                Name = "Create User",
                Group = "User",
                Type = "API",
                SortOrder = 2,
                IsActive = true
            },
            new Permission
            {
                Id = UserUpdateId,
                Code = UserPermissions.UPDATE,
                Name = "Update User",
                Group = "User",
                Type = "API",
                SortOrder = 3,
                IsActive = true
            },
            new Permission
            {
                Id = UserDeleteId,
                Code = UserPermissions.DELETE,
                Name = "Delete User",
                Group = "User",
                Type = "API",
                SortOrder = 4,
                IsActive = true
            },
            new Permission
            {
                Id = UserLockId,
                Code = UserPermissions.LOCK,
                Name = "Lock User",
                Group = "User",
                Type = "API",
                SortOrder = 5,
                IsActive = true
            },
            new Permission
            {
                Id = UserViewLogsId,
                Code = UserPermissions.VIEW_LOGS,
                Name = "View User Logs",
                Group = "User",
                Type = "API",
                SortOrder = 6,
                IsActive = true
            }
        ];
    }
}
