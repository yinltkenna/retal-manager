using IdentityService.src.Domain.Entities;
using IdentityService.src.Domain.Enums;

namespace IdentityService.src.Application.Definitions
{
    public static class UserDefinitions
    {
        public static readonly Guid AdminId = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d");
        public static readonly Guid UserId = Guid.Parse("6f7a8b9c-1d2e-3f4a-5b6c-7d8e9f0a1b2c");
        private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
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
