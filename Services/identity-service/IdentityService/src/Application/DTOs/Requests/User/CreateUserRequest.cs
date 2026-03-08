namespace IdentityService.src.Application.DTOs.Requests.User
{
    // This DTO is used for creating a new user in the system.
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; } // Password default and hash.
        public string UserPhoneNumber { get; set; }

        public Guid TenantId { get; set; }
        public Guid RoleId { get; set; }
    }
}
