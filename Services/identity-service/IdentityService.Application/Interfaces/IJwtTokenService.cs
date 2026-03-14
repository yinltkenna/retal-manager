using IdentityService.Application.DTOs.Responses.Authentication;

namespace IdentityService.Application.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessToken(UserInfo user);
        string GenerateRefreshToken();
    }
}
