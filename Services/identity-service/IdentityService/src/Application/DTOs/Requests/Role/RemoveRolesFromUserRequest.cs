namespace IdentityService.src.Application.DTOs.Requests.Role
{
    public class RemoveRolesFromUserRequest
    {
        public Guid UserId { get; set; }
        public List<Guid> RoleIds { get; set; }
    }
}
