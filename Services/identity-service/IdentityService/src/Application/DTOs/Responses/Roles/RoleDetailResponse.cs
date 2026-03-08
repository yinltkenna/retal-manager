using IdentityService.src.Application.DTOs.Responses.Permessions;

namespace IdentityService.src.Application.DTOs.Responses.Roles
{
    public class RoleDetailResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public List<PermissionResponse>? Permissions { get; set; }
    }
}
