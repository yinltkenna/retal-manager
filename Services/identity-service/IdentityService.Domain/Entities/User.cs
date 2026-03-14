namespace IdentityService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; } // When User join to a room, this field will be set to the Tenant's Id of Tenancy-Service. Otherwise, it will be null.
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Status { get; set; } // Active, Inactive, Banned, etc. (Optional)
        public bool IsActive { get; set; } = false; // Just True when User has join to room.
        public bool IsDeleted { get; set; } = false; // Soft delete.  

        // profile info
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public DateTime? LastPasswordChangedAt { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpiry { get; set; }
        public int? AccessFailedCount { get; set; }
        public DateTime? LockOutEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

    }
}
