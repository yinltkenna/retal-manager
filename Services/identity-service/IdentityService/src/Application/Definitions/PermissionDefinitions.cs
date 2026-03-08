using EventContracts.Authorization;
using IdentityService.src.Domain.Entities;

namespace IdentityService.src.Application.Definitions
{
    public static class PermissionDefinitions
    {
        public static readonly Guid ViewId = Guid.Parse("a8e96f8f-6ae8-4ffb-bb90-5a90786951d1");
        public static readonly Guid CreateId = Guid.Parse("c9a3c1a0-2d0a-4a88-bfa2-72ee4178fd3f");
        public static readonly Guid UpdateId = Guid.Parse("3f9c82c7-aa6e-4b34-9e35-971f2be1a378");
        public static readonly Guid DeleteId = Guid.Parse("7c5d8dc0-7169-4b2e-8d44-9a7bb9fcc0e8");
        public static readonly Guid LockId = Guid.Parse("e8c1e1b3-50ac-4361-a2f1-3736ca3d6a92");
        public static readonly Guid ViewLogsId = Guid.Parse("1b5aeb2f-e5b6-41c4-9c6a-3a8a9acb0d3f");

        public static IEnumerable<Permission> Get() =>
        [
            new Permission
            {
                Id = ViewId,
                Code = UserPermissions.VIEW,
                Name = "View User",
                Group = "User",
                Type = "API",
                SortOrder = 1,
                IsActive = true
            },
            new Permission
            {
                Id = CreateId,
                Code = UserPermissions.CREATE,
                Name = "Create User",
                Group = "User",
                Type = "API",
                SortOrder = 2,
                IsActive = true
            },
            new Permission
            {
                Id = UpdateId,
                Code = UserPermissions.UPDATE,
                Name = "Update User",
                Group = "User",
                Type = "API",
                SortOrder = 3,
                IsActive = true
            },
            new Permission
            {
                Id = DeleteId,
                Code = UserPermissions.DELETE,
                Name = "Delete User",
                Group = "User",
                Type = "API",
                SortOrder = 4,
                IsActive = true
            },
            new Permission
            {
                Id = LockId,
                Code = UserPermissions.LOCK,
                Name = "Lock User",
                Group = "User",
                Type = "API",
                SortOrder = 5,
                IsActive = true
            },
            new Permission
            {
                Id = ViewLogsId,
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
