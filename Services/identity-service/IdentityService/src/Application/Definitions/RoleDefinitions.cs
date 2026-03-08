using IdentityService.src.Domain.Entities;
using IdentityService.src.Domain.Enums;
namespace IdentityService.src.Application.Definitions
{
    public static class RoleDefinitions
    {
        public static readonly Guid RoleAdminId = Guid.Parse("5a1c2e3f-4b6d-4c8e-9f1a-2b3c4d5e6f7a");
        public static readonly Guid RoleUserId = Guid.Parse("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c");
        public static IEnumerable<Role> Get() =>
        [
            new Role
            {
                Id = RoleAdminId,
                Name = RoleEnum.Admin,
                Description = "Admin role with full permissions",
                IsActive = true,
                IsDeleted = false
            },
            new Role
            {
                Id = RoleUserId,
                Name = RoleEnum.User,
                Description = "User role with limited permissions",
                IsActive = true,
                IsDeleted = false
            }
            ];
    }
}
