using AutoMapper;
using EventContracts.Authorization.Permissions.IdentityService;
using IdentityService.src.Application.DTOs.Requests.User;
using IdentityService.src.Application.DTOs.Responses.Permessions;
using IdentityService.src.Application.DTOs.Responses.User;
using IdentityService.src.Application.Interfaces;
using IdentityService.src.Application.Services.Interfaces;
using IdentityService.src.Domain.Entities;
using IdentityService.src.Domain.Enums;
using IdentityService.src.Infrastructure.Caching;
using IdentityService.src.Infrastructure.Messaging.Publishers;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using IdentityService.src.Web.Common.TemplateResponses;
using IdentityService.src.Web.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.src.Application.Services.Implementations
{
    public class UserService(ILogger<UserService> logger,
                             IUserRepository userRepo,
                             IPermissionRepository permissionRepo,
                             IRoleService roleService,
                             IUnitOfWork uow,
                             IRabbitMqPublisher publisher,
                             ICacheService cache,
                             IOptions<JwtSettings> jwtOptions,
                             IPermissionChecker permissionChecker,
                             IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IPermissionRepository _permissionRepo = permissionRepo;
        private readonly IRoleService _roleService = roleService;
        private readonly IUnitOfWork _uow = uow;
        private readonly IRabbitMqPublisher _publisher = publisher;
        private readonly ICacheService _cache = cache;
        private readonly JwtSettings _jwtSettings = jwtOptions.Value;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserService> _logger = logger;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<string>> CreateUserAsync(CreateUserRequest request, Guid currentUserId)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.CREATE))
                {
                    return ApiResponse<string>.FailResponse("Unauthorized");
                }

                // Check username
                bool isExitsUsername = await _userRepo.ExistsByUsernameAsync(request.UserName);
                if (isExitsUsername)
                {
                    return ApiResponse<string>.FailResponse("Username already exists");
                }

                // Check email
                bool isExitsEmail = await _userRepo.ExistsByEmailAsync(request.UserEmail);
                if (isExitsEmail)
                {
                    return ApiResponse<string>.FailResponse("Email already exists");
                }

                // Hash password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.UserPassword);

                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.UserName,
                    Email = request.UserEmail,
                    PhoneNumber = request.UserPhoneNumber ?? string.Empty,
                    PasswordHash = passwordHash,

                    TenantId = null,

                    IsEmailConfirmed = false,
                    IsActive = true,
                    Status = StatusEnum.Active,

                    AccessFailedCount = 0,
                    LockOutEnd = null,

                    LastPasswordChangedAt = DateTime.UtcNow,

                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };

                // Save user
                await _userRepo.AddAsync(newUser);
                await _uow.SaveChangesAsync();

                // Assign role
                if (request.RoleId != Guid.Empty)
                {
                    await _roleService.AssignRoleToUser(request.RoleId, newUser.Id, currentUserId);
                }

                await _uow.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("User created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUserAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error creating user");
            }
        }
        public async Task<ApiResponse<string>> DeleteUserAsync(Guid idToDel, Guid idRequest)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.DELETE))
                {
                    return ApiResponse<string>.FailResponse("Unauthorized");
                }

                if(idToDel != Guid.Empty && idRequest != Guid.Empty) {
                    if(idToDel == idRequest) {
                        return ApiResponse<string>.FailResponse("You cannot delete yourself");
                    }
                } else {
                    return ApiResponse<string>.FailResponse("Invalid user ID");
                }

                var user = await _userRepo.GetByIdAsync(idToDel);
                if (user == null)
                    return ApiResponse<string>.FailResponse("User not found");

                user.IsDeleted = true;
                user.LastUpdatedAt = DateTime.UtcNow;
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("User deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteUserAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error deleting user");
            }
        }

        public async Task<ApiResponse<PagedResponse<UserListResponse>>> GetAllUsers(UserQueryRequest request)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.VIEW))
                {
                    return ApiResponse<PagedResponse<UserListResponse>>.FailResponse("Unauthorized");
                }

                if (request.PageSize <= 0) request.PageSize = 15;

                var query = await _userRepo.GetAllUsers();

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var term = request.SearchTerm.ToLower();
                    query = query.Where(u => u.Username.Contains(term, StringComparison.CurrentCultureIgnoreCase)
                                            || u.Email.Contains(term, StringComparison.CurrentCultureIgnoreCase));
                }

                var total = await query.CountAsync();
                var users = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var data = _mapper.Map<List<UserListResponse>>(users);
                var resp = new PagedResponse<UserListResponse>
                {
                    Data = data,
                    TotalCount = total,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
                return ApiResponse<PagedResponse<UserListResponse>>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllUsers error: {ex.Message}");
                return ApiResponse<PagedResponse<UserListResponse>>.FailResponse("Error querying users");
            }
        }

        public async Task<ApiResponse<UserDetailResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.VIEW))
                {
                    return ApiResponse<UserDetailResponse>.FailResponse("Unauthorized");
                }
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null) return ApiResponse<UserDetailResponse>.FailResponse("User not found");

                var roles = await _userRepo.GetRolesByUserIdAsync(id);
                var detail = _mapper.Map<UserDetailResponse>(user);
                detail.Roles = roles?.Select(r => r.ToString()).ToList();
                if (roles != null && roles.Count > 0)
                {
                    var perms = await _permissionRepo.GetPermissionsByRoleIdsAsync(roles);
                    detail.Permissions = _mapper.Map<List<PermissionResponse>>(perms);
                }

                return ApiResponse<UserDetailResponse>.SuccessResponse(detail);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetByIdAsync error: {ex.Message}");
                return ApiResponse<UserDetailResponse>.FailResponse("Error fetching user");
            }
        }

        public async Task<ApiResponse<UserProfileResponse>> GetProfileAsync(Guid userId)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.VIEW))
                {
                    return ApiResponse<UserProfileResponse>.FailResponse("Unauthorized");
                }
                var user = await _userRepo.GetByIdAsync(userId);
                if (user == null) return ApiResponse<UserProfileResponse>.FailResponse("User not found");
                var profile = _mapper.Map<UserProfileResponse>(user);
                return ApiResponse<UserProfileResponse>.SuccessResponse(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProfileAsync error: {ex.Message}");
                return ApiResponse<UserProfileResponse>.FailResponse("Error fetching profile");
            }
        }

        public async Task<ApiResponse<string>> LockUserAsync(Guid id)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.LOCK))
                {
                    return ApiResponse<string>.FailResponse("Unauthorized");
                }
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null) return ApiResponse<string>.FailResponse("User not found");

                // Lock until 100 years later (effectively permanent lock)
                user.LockOutEnd = DateTime.UtcNow.AddYears(100);
                user.LastUpdatedAt = DateTime.UtcNow;
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("User locked");
            }
            catch (Exception ex)
            {
                _logger.LogError($"LockUserAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error locking user");
            }
        }

        public async Task<ApiResponse<string>> UnlockUserAsync(Guid id)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.LOCK))
                {
                    return ApiResponse<string>.FailResponse("Unauthorized");
                }
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null) return ApiResponse<string>.FailResponse("User not found");

                user.LockOutEnd = null;
                user.LastUpdatedAt = DateTime.UtcNow;
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("User unlocked");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnlockUserAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error unlocking user");
            }
        }

        public async Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.UPDATE))
                {
                    return ApiResponse<string>.FailResponse("Unauthorized");
                }
                var user = await _userRepo.GetByIdAsync(userId);
                if (user == null) return ApiResponse<string>.FailResponse("User not found");

                if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
                {
                    if (await _userRepo.ExistsByEmailAsync(request.Email))
                        return ApiResponse<string>.FailResponse("Email already in use");
                    user.Email = request.Email;
                }
                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    user.PhoneNumber = request.PhoneNumber;
                }
                user.LastUpdatedAt = DateTime.UtcNow;
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Profile updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateProfileAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error updating profile");
            }
        }

        public async Task<ApiResponse<string>> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            try
            {
                if (!await _permissionChecker.HasPermissionAsync(UserPermissions.UPDATE))
                {
                    return ApiResponse<string>.FailResponse("Unauthorized");
                }
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null) return ApiResponse<string>.FailResponse("User not found");

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                    user.PhoneNumber = request.PhoneNumber;
                if (request.IsActive.HasValue)
                    user.IsActive = request.IsActive.Value;
                
                user.LastUpdatedAt = DateTime.UtcNow;
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("User updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateUserAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error updating user");
            }
        }
    }
}