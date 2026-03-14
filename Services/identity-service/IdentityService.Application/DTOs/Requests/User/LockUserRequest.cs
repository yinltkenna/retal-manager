namespace IdentityService.Application.DTOs.Requests.User
{
    public class LockUserRequest
    {
        public Guid UserId { get; set; }
        public bool IsLocked { get; set; } // True to lock the user, False to unlock the user.
    }
}
