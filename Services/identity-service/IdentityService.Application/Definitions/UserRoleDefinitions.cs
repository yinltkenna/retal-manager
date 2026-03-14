using IdentityService.Domain.Entities;

namespace IdentityService.Application.Definitions
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
