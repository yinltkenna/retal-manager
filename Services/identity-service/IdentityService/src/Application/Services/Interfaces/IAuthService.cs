using IdentityService.src.Application.DTOs.Requests.Authentication;
using IdentityService.src.Application.DTOs.Responses.Authentication;
using IdentityService.src.Web.Common.TemplateResponses;

namespace IdentityService.src.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> Login(LoginRequest request);
        Task<ApiResponse<string>> RegisterTenant(RegisterTenantRequest request);
        Task<ApiResponse<string>> Logout(LogoutRequest request);
        Task<ApiResponse<LoginResponse>> RefreshToken(RefreshTokenRequest request);
        Task<ApiResponse<string>> ConfirmEmail(ConfirmEmailRequest request);
        Task<ApiResponse<string>> ForgotPassword(ForgotPasswordRequest request); // To send reset password email
        Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest request); // Confirm the reset password using token and set new password
        Task<ApiResponse<string>> ChangePassword(Guid userId, ChangePasswordRequest request); // Change password for logged in user
    }
}
