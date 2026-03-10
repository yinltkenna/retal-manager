using AutoMapper;
using IdentityService.src.Application.DTOs.Requests.Authentication;
using IdentityService.src.Application.DTOs.Responses.Authentication;
using IdentityService.src.Application.Services.Interfaces;
using IdentityService.src.Domain.Entities;
using IdentityService.src.Domain.Enums;
using IdentityService.src.Infrastructure.Caching;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using IdentityService.src.Web.Common.TemplateResponses;
using System.Security.Cryptography;

namespace IdentityService.src.Application.Services.Implementations
{
    public class AuthService(ILogger<AuthService> logger,
                             IMapper mapper,
                             IUserRepository userRepository,
                             IRoleRepository roleRepository,
                             IRoleService roleService,
                             IRefreshTokenRepository refreshTokenRepository,
                             IPermissionRepository permissionRepository,
                             IEmailVerificationTokenRepository emailTokenRepository,
                             INotificationClient notificationClient,
                             IJwtTokenService jwtTokenService,
                             ITenancyServiceClient tenancyServiceClient,
                             IUnitOfWork unitOfWork,
                             ICacheService cacheService) : IAuthService
    {
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IRoleService _roleService = roleService;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;
        private readonly IEmailVerificationTokenRepository _emailTokenRepository = emailTokenRepository;
        private readonly INotificationClient _notificationClient = notificationClient;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly ITenancyServiceClient _tenancyServiceClient = tenancyServiceClient;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<ApiResponse<string>> ChangePassword(Guid userId, ChangePasswordRequest request)
        {
            try
            {
                // Get user
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    return ApiResponse<string>.FailResponse("User not found");
                }

                // Check user status
                if (!user.IsActive || user.Status != StatusEnum.Active)
                {
                    return ApiResponse<string>.FailResponse("User account is inactive");
                }

                // Verify current password
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                    request.CurrentPassword,
                    user.PasswordHash
                );

                if (!isValidPassword)
                {
                    return ApiResponse<string>.FailResponse("Current password is incorrect");
                }

                // Hash new password
                string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                // Update user password
                user.PasswordHash = newPasswordHash;
                user.LastPasswordChangedAt = DateTime.UtcNow;
                user.LastUpdatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"User {user.Id} changed password successfully");


                // Revoke all existing refresh tokens for the user
                var refreshTokens = await _refreshTokenRepository.GetByUserIdAsync(user.Id);
                if (refreshTokens.Count == 0)
                {
                    _logger.LogWarning($"No refresh tokens found for user {user.Id} during password change");
                }
                else
                {
                    foreach (var token in refreshTokens)
                    {
                        token.IsRevoked = true;
                        token.RevokedAt = DateTime.UtcNow;

                        _refreshTokenRepository.Update(token);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"Revoked {refreshTokens.Count} refresh tokens for user {user.Id} after password change");
                }
                return ApiResponse<string>.SuccessResponse("Password changed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");

                return ApiResponse<string>.FailResponse("An error occurred while changing password");
            }
        }
        public async Task<ApiResponse<string>> ConfirmEmail(ConfirmEmailRequest request)
        {
            try
            {
                var tokenEntity = await _emailTokenRepository.GetByTokenAsync(request.Token);
                if (tokenEntity == null || tokenEntity.IsUsed || tokenEntity.ExpiresAt <= DateTime.UtcNow)
                    return ApiResponse<string>.FailResponse("Invalid or expired token");

                // ensure email matches user
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null || user.Id != tokenEntity.UserId)
                    return ApiResponse<string>.FailResponse("Token does not match user");

                user.IsEmailConfirmed = true;
                tokenEntity.IsUsed = true;
                tokenEntity.UsedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(user); // will be saved by uow
                _emailTokenRepository.Update(tokenEntity);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("Email confirmed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"ConfirmEmail error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error confirming email");
            }
        }
        public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                {
                    // for security, still respond success
                    return ApiResponse<string>.SuccessResponse(string.Empty);
                }

                // generate simple token
                var token = Guid.NewGuid().ToString();
                user.ResetPasswordToken = token;
                user.ResetPasswordExpiry = DateTime.UtcNow.AddHours(1);
                await _notificationClient.SendEmailAsync(user.Email, "Reset your password", $"Your token: {token}");

                await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("", "If the email exists, a reset link has been sent");
            }
            catch (Exception ex)
            {
                _logger.LogError($"ForgotPassword error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error processing request");
            }
        }
        public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                _logger.LogInformation($"Login attempt for user: {request.Username}");

                // Find user by email or username
                User? user = null;
                user = await _userRepository.GetByUsernameAsync(request.Username);

                if (user == null)
                {
                    _logger.LogWarning($"Login failed: User not found - {request.Username}");
                    return ApiResponse<LoginResponse>.FailResponse("Invalid username or password");
                }

                // Verify password using BCrypt
                bool passwordValid = BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash);
                if (!passwordValid)
                {
                    _logger.LogWarning($"Login failed: Invalid password for user - {user.Id}");
                    return ApiResponse<LoginResponse>.FailResponse("Invalid email/username or password");
                }

                // Get user roles (list of role IDs)
                var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);
                // Validate roles
                if (roles == null || roles.Count == 0)
                {
                    _logger.LogWarning($"Login failed: No roles found for user - {user.Username}");
                    return ApiResponse<LoginResponse>.FailResponse("User has no roles assigned");
                }
                var roleIds = roles.Select(r => r.Id).ToList();
                // Get permissions of roles by ID list
                var permissions = await _permissionRepository.GetPermissionsByRoleIdsAsync(roleIds);

                // permission store in cache
                await _cacheService.SetAsync(user.Id.ToString(), permissions, TimeSpan.FromDays(2));
                // Build UserInfo
                var userInfo = new UserInfo
                {
                    UserId = user.Id,
                    UserName = user.Username,
                    TenantId = user.TenantId,
                    Roles = roleIds,
                };

                // Generate tokens
                var accessToken = _jwtTokenService.GenerateAccessToken(userInfo);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                // Save refresh token to database
                var refreshTokenEntity = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = BCrypt.Net.BCrypt.HashPassword(refreshToken),
                    ExpiresAt = DateTime.UtcNow.AddDays(2), // Refresh token validity: 2 days
                    CreatedAt = DateTime.UtcNow
                };

                await _refreshTokenRepository.AddAsync(refreshTokenEntity);
                await _unitOfWork.SaveChangesAsync();

                // Return response
                var response = new LoginResponse
                {
                    AccessToken = await accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60), // Access token expires in 60 minutes
                    User = userInfo
                };

                _logger.LogInformation($"Login successful for user: {user.Username}");
                return ApiResponse<LoginResponse>.SuccessResponse(response, "Login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return ApiResponse<LoginResponse>.FailResponse("An error occurred during login");
            }
        }
        public async Task<ApiResponse<string>> Logout(LogoutRequest request)
        {
            try
            {
                var existing = await _refreshTokenRepository.GetByRawTokenAsync(request.RefreshToken);
                if (existing != null)
                {
                    existing.IsRevoked = true;
                    existing.RevokedAt = DateTime.UtcNow;
                    _refreshTokenRepository.Update(existing);
                    await _unitOfWork.SaveChangesAsync();
                }
                return ApiResponse<string>.SuccessResponse("Logged out");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logout error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error during logout");
            }
        }
        public async Task<ApiResponse<LoginResponse>> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return ApiResponse<LoginResponse>.FailResponse("Refresh token is required");
                }

                // Find refresh token (verify raw token vs hashed token)
                var storedToken = await _refreshTokenRepository.GetByRawTokenAsync(request.RefreshToken);

                if (storedToken == null)
                {
                    return ApiResponse<LoginResponse>.FailResponse("Invalid refresh token");
                }

                if (storedToken.IsRevoked)
                {
                    return ApiResponse<LoginResponse>.FailResponse("Refresh token revoked");
                }

                if (storedToken.IsExpired)
                {
                    return ApiResponse<LoginResponse>.FailResponse("Refresh token expired");
                }

                // Get user
                var user = await _userRepository.GetByIdAsync(storedToken.UserId);

                if (user == null || !user.IsActive)
                {
                    return ApiResponse<LoginResponse>.FailResponse("User not found or inactive");
                }

                // Build user info for JWT
                var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);

                var userInfo = new UserInfo
                {
                    UserId = user.Id,
                    UserName = user.Username,
                    TenantId = user.TenantId,
                    Roles = [.. roles.Select(r => r.Id)]
                };

                // Generate new access token
                var accessToken = await _jwtTokenService.GenerateAccessToken(userInfo);

                // Generate new refresh token (rotation)
                var rawRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                var hashedRefreshToken = BCrypt.Net.BCrypt.HashPassword(rawRefreshToken);

                var newRefreshToken = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = hashedRefreshToken,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7) // Default refresh token expiration
                };

                // Revoke old refresh token
                storedToken.IsRevoked = true;
                storedToken.RevokedAt = DateTime.UtcNow;
                storedToken.ReplacedByToken = newRefreshToken.Id.ToString();

                _refreshTokenRepository.Update(storedToken);

                // Save new refresh token
                await _refreshTokenRepository.AddAsync(newRefreshToken);

                await _unitOfWork.SaveChangesAsync();

                var response = new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = rawRefreshToken
                };

                return ApiResponse<LoginResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return ApiResponse<LoginResponse>.FailResponse("Error refreshing token");
            }
        }
        public async Task<ApiResponse<string>> RegisterTenant(RegisterTenantRequest request)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Check email
                bool emailExists = await _userRepository.ExistsByEmailAsync(request.Email);
                if (emailExists)
                {
                    _logger.LogWarning($"RegisterTenant failed: Email already exists - {request.Email}");
                    return ApiResponse<string>.FailResponse("Email already registered");
                }

                // Check username
                bool usernameExists = await _userRepository.ExistsByUsernameAsync(request.Username);
                if (usernameExists)
                {
                    _logger.LogWarning($"RegisterTenant failed: Username already exists - {request.Username}");
                    return ApiResponse<string>.FailResponse("Username already registered");
                }

                // Hash password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Create new user
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    PasswordHash = hashedPassword,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    TenantId = null,
                    IsActive = true,
                    IsEmailConfirmed = false,
                    Status = StatusEnum.Active,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };

                // Save user first
                await _userRepository.AddAsync(newUser);
                await _unitOfWork.SaveChangesAsync();

                // Default role assignment
                var role = await _roleRepository.GetByNameAsync(RoleEnum.User); // Get default "User" role
                if (role == null)
                {
                    _logger.LogError($"Default role '{RoleEnum.User}' not found in database");
                    return ApiResponse<string>.FailResponse("Default role not configured");
                }
                await _roleService.AssignRoleToUser(role.Id, newUser.Id, Guid.Empty);

                // If user provided CodeContract -> try claim contract
                if (!string.IsNullOrWhiteSpace(request.CodeContract))
                {
                    var tenantId = await _tenancyServiceClient
                        .ClaimContractAsync(request.CodeContract, newUser.Id);

                    if (tenantId != null)
                    {
                        newUser.TenantId = tenantId;

                        await _userRepository.UpdateAsync(newUser);
                        await _unitOfWork.SaveChangesAsync();

                        _logger.LogInformation($"User {newUser.Id} successfully linked to contract");
                    }
                    else
                    {
                        _logger.LogWarning($"RegisterTenant: Contract invalid or already claimed - {request.CodeContract}");
                    }
                }

                _logger.LogInformation($"RegisterTenant successful for email: {request.Email}");
                await transaction.CommitAsync();
                return ApiResponse<string>.SuccessResponse(
                    newUser.Id.ToString(),
                    "Registration successful"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"RegisterTenant error: {ex.Message}");
                return ApiResponse<string>.FailResponse("An error occurred during registration");
            }
        }
        public async Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                    return ApiResponse<string>.FailResponse("Invalid request");

                if (user.ResetPasswordToken != request.Token ||
                    user.ResetPasswordExpiry == null ||
                    user.ResetPasswordExpiry < DateTime.UtcNow)
                {
                    return ApiResponse<string>.FailResponse("Invalid or expired token");
                }

                if (request.NewPassword != request.ConfirmPassword)
                    return ApiResponse<string>.FailResponse("Passwords do not match");

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.ResetPasswordToken = null;
                user.ResetPasswordExpiry = null;
                user.LastPasswordChangedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("Password has been reset");
            }
            catch (Exception ex)
            {
                _logger.LogError($"ResetPassword error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error resetting password");
            }
        }
    }
}
