using EventContracts.Authorization;
using IdentityService.src.Domain.Entities;
using IdentityService.src.Domain.Enums;
namespace IdentityService.src.Application.Definitions
{
    public static class RoleDefinitions
    {
        public static readonly Guid RoleAdminId = DefId.AdminId;
        public static readonly Guid RoleUserId = DefId.UserId;
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
