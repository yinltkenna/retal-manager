namespace IdentityService.Application.DTOs.Requests.Permession
{
    public class RemovePermissionsToRoleRequest
    {
        public List<Guid> RoleIds { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }
}
