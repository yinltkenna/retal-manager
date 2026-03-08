using IdentityService.src.Application.DTOs.Responses.Authentication;

namespace IdentityService.src.Application.Services.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateAccessToken(UserInfo user);
        string GenerateRefreshToken();
    }
}
