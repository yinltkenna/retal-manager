namespace IdentityService.Application.DTOs.Requests.Role
{
    public class AssignRolesToUserRequest
    {
        public List<Guid> UserId { get; set; }
        public List<Guid> RoleIds { get; set; }
    }
}
