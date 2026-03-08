namespace IdentityService.src.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Token { get; set; } // Hash token value for security

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public string? ReplacedByToken { get; set; } // nếu dùng rotation

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
