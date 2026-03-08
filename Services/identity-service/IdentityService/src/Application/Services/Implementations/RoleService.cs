using AutoMapper;
using IdentityService.src.Application.DTOs.Requests.Role;
using IdentityService.src.Application.DTOs.Responses.Permessions;
using IdentityService.src.Application.DTOs.Responses.Roles;
using IdentityService.src.Application.Services.Interfaces;
using IdentityService.src.Domain.Entities;
using IdentityService.src.Infrastructure.Data;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using IdentityService.src.Web.Common.TemplateResponses;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.src.Application.Services.Implementations
{
    public class RoleService(ILogger<RoleService> logger,
                             IRoleRepository roleRepo,
                             IUserRepository userRepo,
                             IPermissionRepository permissionRepo,
                             AppDbContext db,
                             IUnitOfWork uow,
                             IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepo = roleRepo;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IPermissionRepository _permissionRepo = permissionRepo;
        private readonly AppDbContext _db = db;
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<RoleService> _logger = logger;

        /// <summary>
        /// Create a new role. Role name must be unique. Returns the created role details if successful, otherwise returns an error message.
        /// </summary>
        /// <param name="request">The request object containing the role details.</param>
        /// <returns>An ApiResponse containing the created role details or an error message.</returns>
        public async Task<ApiResponse<RoleDetailResponse?>> CreateAsync(CreateRoleRequest request)
        {
            try
            {
                var exists = await _roleRepo.GetByNameAsync(request.Name);
                if (exists != null)
                    return ApiResponse<RoleDetailResponse?>.FailResponse("Role name already exists");

                var role = _mapper.Map<Role>(request);
                if (role != null)
                {
                    role.Id = Guid.NewGuid();
                    role.IsActive = true;
                    role.IsDeleted = false;
                    await _roleRepo.AddAsync(role);
                    await _uow.SaveChangesAsync();
                }
                var resp = _mapper.Map<RoleDetailResponse>(role);
                return ApiResponse<RoleDetailResponse?>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateAsync error: {ex.Message}");
                return ApiResponse<RoleDetailResponse?>.FailResponse("Error creating role");
            }
        }

        /// <summary>
        /// Update an existing role. Only non-null fields in the request will be updated. Returns the updated role details if successful, otherwise returns an error message.
        /// </summary>
        /// <param name="id">The ID of the role to update.</param>
        /// <param name="request">The request object containing the updated role details.</param>
        /// <returns>An ApiResponse containing the updated role details or an error message.</returns>
        public async Task<ApiResponse<RoleDetailResponse?>> UpdateAsync(Guid id, UpdateRoleRequest request)
        {
            try
            {
                var role = await _roleRepo.GetByIdAsync(id);
                if (role == null)
                    return ApiResponse<RoleDetailResponse?>.FailResponse("Role not found");

                if (!string.IsNullOrWhiteSpace(request.Name))
                    role.Name = request.Name;
                if (!string.IsNullOrWhiteSpace(request.Description))
                    role.Description = request.Description;

                _roleRepo.Update(role);
                await _uow.SaveChangesAsync();
                var resp = _mapper.Map<RoleDetailResponse>(role);
                return ApiResponse<RoleDetailResponse?>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateAsync error: {ex.Message}");
                return ApiResponse<RoleDetailResponse?>.FailResponse("Error updating role");
            }
        }

        /// <summary>
        /// Delete an existing role. Marks the role as deleted. Returns true if successful, otherwise returns an error message.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>An ApiResponse containing a boolean indicating success or failure.</returns>
        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var role = await _roleRepo.GetByIdAsync(id);
                if (role == null)
                    return ApiResponse<bool>.FailResponse("Role not found");
                role.IsDeleted = true;
                await _uow.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteAsync error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error deleting role");
            }
        }

        /// <summary>
        /// Get all roles. Returns a paged response containing the role details.
        /// </summary>
        /// <returns>An ApiResponse containing a paged response of role details.</returns>
        public async Task<ApiResponse<PagedResponse<RoleDetailResponse>>> GetAllAsync()
        {
            try
            {
                var roles = await _roleRepo.GetAllAsync();
                var data = _mapper.Map<List<RoleDetailResponse>>(roles);
                if (data == null || data.Count == 0)
                { return ApiResponse<PagedResponse<RoleDetailResponse>>.FailResponse("No roles found"); }
                // default page size 15
                var paged = new PagedResponse<RoleDetailResponse>
                {
                    Data = data,
                    TotalCount = data.Count,
                    PageNumber = 1,
                    PageSize = 15
                };
                return ApiResponse<PagedResponse<RoleDetailResponse>>.SuccessResponse(paged);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllAsync error: {ex.Message}");
                return ApiResponse<PagedResponse<RoleDetailResponse>>.FailResponse("Error retrieving roles");
            }
        }
        /// <summary>
        /// Get role details by ID. Returns the role details if found, otherwise returns an error message.
        /// </summary>
        /// <param name="id">The ID of the role to retrieve.</param>
        /// <returns>An ApiResponse containing the role details if found, otherwise an error message.</returns>
        public async Task<ApiResponse<RoleDetailResponse?>> GetByIdAsync(Guid id)
        {
            try
            {
                var role = await _roleRepo.GetByIdAsync(id);
                if (role == null)
                    return ApiResponse<RoleDetailResponse?>.FailResponse("Role not found");

                var resp = _mapper.Map<RoleDetailResponse>(role);
                var perms = await _roleRepo.GetPermissionsByRoleIdAsync(id);
                resp.Permissions = _mapper.Map<List<PermissionResponse>>(perms);
                return ApiResponse<RoleDetailResponse?>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetByIdAsync error: {ex.Message}");
                return ApiResponse<RoleDetailResponse?>.FailResponse("Error retrieving role");
            }
        }

        public async Task<ApiResponse<bool>> AssignRoleToUser(Guid roleId, Guid userId)
        {
            try
            {
                var user = await _userRepo.GetByIdAsync(userId);
                if (user == null) return ApiResponse<bool>.FailResponse("User not found");
                var role = await _roleRepo.GetByIdAsync(roleId);
                if (role == null) return ApiResponse<bool>.FailResponse("Role not found");

                bool exists = await _db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
                if (!exists)
                {
                    _db.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
                    await _uow.SaveChangesAsync();
                }
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AssignRoleToUser error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error assigning role to user");
            }
        }

        public async Task<ApiResponse<bool>> AssignRoleToUser(List<Guid> roleId, List<Guid> userIds)
        {
            try
            {
                foreach (var r in roleId)
                {
                    foreach (var u in userIds)
                    {
                        bool exists = await _db.UserRoles.AnyAsync(ur => ur.UserId == u && ur.RoleId == r);
                        if (!exists)
                            _db.UserRoles.Add(new UserRole { UserId = u, RoleId = r });
                    }
                }
                await _uow.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AssignRoleToUser (bulk) error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error assigning roles to users");
            }
        }

        public async Task<ApiResponse<bool>> RemoveRoleFromUser(List<Guid> roleId, List<Guid> userIds)
        {
            try
            {
                var toRemove = _db.UserRoles.Where(ur => userIds.Contains(ur.UserId) && roleId.Contains(ur.RoleId));
                _db.UserRoles.RemoveRange(toRemove);
                await _uow.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"RemoveRoleFromUser error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error removing roles from users");
            }
        }

        public async Task<ApiResponse<bool>> AssignPermissionsToRoles(List<Guid> permissionId, List<Guid> roleIds)
        {
            try
            {
                foreach (var r in roleIds)
                {
                    foreach (var p in permissionId)
                    {
                        bool exists = await _db.RolePermissions.AnyAsync(rp => rp.RoleId == r && rp.PermissionId == p);
                        if (!exists)
                            _db.RolePermissions.Add(new RolePermission { RoleId = r, PermissionId = p });
                    }
                }
                await _uow.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AssignPermissionToRole error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error assigning permissions to role");
            }
        }

        public async Task<ApiResponse<bool>> RemovePermissionFromRole(List<Guid> permissionId, List<Guid> roleIds)
        {
            try
            {
                var toRemove = _db.RolePermissions.Where(rp => roleIds.Contains(rp.RoleId) && permissionId.Contains(rp.PermissionId));
                _db.RolePermissions.RemoveRange(toRemove);
                await _uow.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"RemovePermissionFromRole error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error removing permissions from role");
            }
        }

        public async Task<ApiResponse<RoleDetailResponse?>> GetByNameAsync(string name)
        {
            try
            {
                var role = await _roleRepo.GetByNameAsync(name);
                if (role == null)
                    return ApiResponse<RoleDetailResponse?>.FailResponse($"Not found role with name {name}");
                var resp = _mapper.Map<RoleDetailResponse>(role);
                var perms = await _roleRepo.GetPermissionsByRoleIdAsync(role.Id);
                resp.Permissions = _mapper.Map<List<PermissionResponse>>(perms);
                return ApiResponse<RoleDetailResponse?>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetByNameAsync error: {ex.Message}");
                return ApiResponse<RoleDetailResponse?>.FailResponse($"Error retrieving role with name {name}");
            }
        }

        public async Task<ApiResponse<List<RoleDetailResponse>>> GetRolesByUserIdAsync(Guid userId)
        {
            try
            {
                var roles = await _roleRepo.GetRolesByUserIdAsync(userId);
                var resp = _mapper.Map<List<RoleDetailResponse>>(roles);
                return ApiResponse<List<RoleDetailResponse>>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRolesByUserIdAsync error: {ex.Message}");
                return ApiResponse<List<RoleDetailResponse>>.FailResponse("Error retrieving roles for the user");
            }
        }
    }
}
