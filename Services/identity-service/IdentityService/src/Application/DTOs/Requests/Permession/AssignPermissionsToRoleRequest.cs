namespace IdentityService.src.Application.DTOs.Requests.Permession
{
    public class AssignPermissionsToRoleRequest
    {
        public List<Guid> RoleIds { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }
}
