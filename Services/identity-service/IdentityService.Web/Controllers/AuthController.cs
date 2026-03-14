using IdentityService.Application.DTOs.Requests.Authentication;
using IdentityService.Application.Interfaces;
using IdentityService.Web.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Login with username and password
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Login(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Register a new tenant user with optional contract code
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterTenant([FromBody] RegisterTenantRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterTenant(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Logout user by revoking refresh token
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Logout(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Confirm email with token and email
        /// </summary>
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ConfirmEmail(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Request password reset token (sends email)
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ForgotPassword(request);
            return Ok(result); // Always return OK for security
        }

        /// <summary>
        /// Reset password with token
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPassword(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = new GetUserIdFromToken().UserIdFromToken;

            if (currentUserId == null)
                return Unauthorized();

            var result = await _authService.ChangePassword(currentUserId.Value, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.RefreshToken(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
