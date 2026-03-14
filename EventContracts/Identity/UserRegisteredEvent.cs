namespace EventContracts.Identity
{
    public class UserRegisteredEvent
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string VerificationToken { get; set; } = string.Empty;
        public DateTime TokenExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}