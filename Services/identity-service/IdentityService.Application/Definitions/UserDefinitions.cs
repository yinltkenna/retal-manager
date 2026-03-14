using EventContracts.Authorization;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Enums;

namespace IdentityService.Application.Definitions
{
    public static class UserDefinitions
    {
        public static readonly Guid AdminId = DefId.AdminId;
        public static readonly Guid UserId = DefId.UserId;
        private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly string PasswordHash = "$2a$11$AEB8w8vo4lFGC971/BTZeu1s3eBRZ49bLuz7ZIRB.oMDlvsAHqGwe"; // yin@Kenna117
        public static IEnumerable<User> Get() =>
        [
            new User
            {
                Id = AdminId,
                Username = "adminApp",
                Email = "admin@example.com",
                PasswordHash = PasswordHash,
                Status = StatusEnum.Active,
                IsActive = false,
                IsDeleted = false,
                PhoneNumber = "1234567890",
                IsEmailConfirmed = true,
                CreatedAt = SeedDate,
                LastUpdatedAt = SeedDate
                },
            new User
            {
                Id = UserId,
                Username = "userApp",
                Email = "userApp@gmail.com",
                PasswordHash = PasswordHash,
                Status = StatusEnum.Active,
                IsActive = false,
                IsDeleted = false,
                PhoneNumber = "1234567880",
                IsEmailConfirmed = true,
                CreatedAt = SeedDate,
                LastUpdatedAt = SeedDate
            }
        ];
    }
}
