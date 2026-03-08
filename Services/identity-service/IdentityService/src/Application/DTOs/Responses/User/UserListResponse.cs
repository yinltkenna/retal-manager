namespace IdentityService.src.Application.DTOs.Responses.User
{
    public class UserListResponse
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; } // When User join to a room, this field will be set to the Tenant's Id of Tenancy-Service. Otherwise, it will be null.
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
