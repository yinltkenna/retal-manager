namespace IdentityService.src.Domain.Entities
{
    public class EmailVerificationToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }
    }
}
