using IdentityService.src.Application.DTOs.Responses.Authentication;
using IdentityService.src.Application.Services.Interfaces;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using IdentityService.src.Web.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.src.Application.Services.Implementations
{
    public class JwtTokenService(IOptions<JwtSettings> jwtSettings,
                                 ILogger<JwtTokenService> logger,
                                 IPermissionRepository permissionRepository) : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;
        private readonly ILogger<JwtTokenService> _logger = logger;

        /// <summary>
        /// Generates a JWT access token for the given user, including their roles and permissions as claims.
        /// </summary>
        /// <param name="user">The user information for which to generate the token.</param>
        /// <returns>UserId, UserName, TenantId, Roles, Permissions</returns>
        public async Task<string> GenerateAccessToken(UserInfo user)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new("TenantId", user.TenantId?.ToString() ?? ""),
                };

                // Add roles
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                // Get permission of roles
                var permissions = await _permissionRepository.GetPermissionsByRoleIdsAsync(user.Roles);

                // Query permissions distinct of roles and add to claims
                var permissionCodes = permissions
                    .Select(p => p.Code?.Trim().ToLowerInvariant())
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct();
                // Result e.g.: permissions: "room.create,room.update,room.delete"
                claims.Add(new Claim("permissions", string.Join(",", permissionCodes)));

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating access token: {ex.Message}");
                throw;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
