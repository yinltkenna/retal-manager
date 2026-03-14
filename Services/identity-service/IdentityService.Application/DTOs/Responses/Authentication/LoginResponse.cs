namespace IdentityService.Application.DTOs.Responses.Authentication
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } // The JWT access token issued to the client after a successful login.
        public string RefreshToken { get; set; } // The refresh token that can be used to obtain a new access token when the current one expires.
        public DateTime ExpiresAt { get; set; }
        public UserInfo User { get; set; }
    }

    // This class is used to return user information along with the token and its expiration time after a successful login.
    // Just Login only
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid? TenantId { get; set; }
        public List<Guid> Roles { get; set; } = new List<Guid>();
    }
}
