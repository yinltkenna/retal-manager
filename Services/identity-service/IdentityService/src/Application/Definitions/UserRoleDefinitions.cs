using IdentityService.src.Domain.Entities;

namespace IdentityService.src.Application.Definitions
{
    public static class UserRoleDefinitions
    {
        public static IEnumerable<UserRole> Get() =>
            [
                new UserRole
                {
                    UserId = UserDefinitions.AdminId,
                    RoleId = RoleDefinitions.RoleAdminId
                },
                new UserRole
                {
                    UserId = UserDefinitions.UserId,
                    RoleId = RoleDefinitions.RoleUserId
                }
            ];
    }
}
