using AutoMapper;
using IdentityService.Application.Common.TemplateResponses;
using IdentityService.Application.DTOs.Requests.Role;
using IdentityService.Application.DTOs.Responses.Permessions;
using IdentityService.Application.DTOs.Responses.Roles;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Services
{
    public class RoleService(ILogger<RoleService> logger,
                             IRoleRepository roleRepo,
                             IUserRepository userRepo,
                             IPermissionRepository permissionRepo,
                             IUnitOfWork uow,
                             IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepo = roleRepo;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IPermissionRepository _permissionRepo = permissionRepo;
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

        public async Task<ApiResponse<bool>> AssignRoleToUser(Guid roleId, Guid userIdBeAssign, Guid userIdRequest)
        {
            try
            {
                if(userIdBeAssign == userIdRequest)
                    return ApiResponse<bool>.FailResponse("You cannot assign role to yourself");

                var exists = await _roleRepo.ExistsRoleAsync(userIdBeAssign, roleId);
                if (exists)
                    return ApiResponse<bool>.FailResponse("User already has this role");

                var success = await _roleRepo.AssignRoleToUserAsync(userIdBeAssign, roleId);
                if (success)
                {
                    await _uow.SaveChangesAsync();
                    _logger.LogInformation("User {UserId} assigned role {RoleId} by {AdminId}", userIdBeAssign, roleId, userIdRequest);
                    return ApiResponse<bool>.SuccessResponse(true, "Role assigned successfully");
                }

                return ApiResponse<bool>.FailResponse("Assigning failed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AssignRoleToUser error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error assigning role to user");
            }
        }

        public async Task<ApiResponse<bool>> AssignRoleToUser(List<Guid> roleIds, List<Guid> userIdsBeAssign, Guid userIdRequest)
        {
            try
            {
                if (userIdsBeAssign.Contains(userIdRequest))
                    return ApiResponse<bool>.FailResponse("You cannot assign role to yourself");
                var existingRoles = await _roleRepo.GetExistingUserRolesAsync(userIdsBeAssign, roleIds);

                var existingSet = new HashSet<(Guid, Guid)>(
                    existingRoles.Select(x => (x.UserId, x.RoleId))
                );

                var rolesToAdd = new List<UserRole>();
                foreach (var uId in userIdsBeAssign)
                {
                    foreach (var rId in roleIds)
                    {
                        if (!existingSet.Contains((uId, rId)))
                        {
                            rolesToAdd.Add(new UserRole { UserId = uId, RoleId = rId });
                        }
                    }
                }

                if (rolesToAdd.Count != 0)
                {
                    await _roleRepo.AddUserRolesRangeAsync(rolesToAdd);
                    await _uow.SaveChangesAsync();

                    _logger.LogInformation("Admin {AdminId} assigned {Count} roles to multiple users", userIdRequest, rolesToAdd.Count);
                }

                return ApiResponse<bool>.SuccessResponse(true, "Process completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AssignRoleToUser (bulk) error: {ex.Message}");
                return ApiResponse<bool>.FailResponse("Error assigning roles to users");
            }
        }

        public async Task<ApiResponse<bool>> RemoveRoleFromUser(List<Guid> roleId, List<Guid> userIdsBeRemove, Guid userIdRequest)
        {
            try
            {
                if (userIdsBeRemove.Contains(userIdRequest))
                {
                    return ApiResponse<bool>.FailResponse("You cannot remove roles from yourself");
                }

                await _roleRepo.RemoveUserRolesRangeAsync(roleId, userIdsBeRemove);

                await _uow.SaveChangesAsync();

                _logger.LogInformation("Admin {AdminId} removed multiple roles from a list of users", userIdRequest);

                return ApiResponse<bool>.SuccessResponse(true, "Roles removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoveRoleFromUser error: {Message}", ex.Message);
                return ApiResponse<bool>.FailResponse("Error removing roles from users");
            }
        }
        public async Task<ApiResponse<bool>> AssignPermissionsToRoles(List<Guid> permissionIds, List<Guid> roleIds)
        {
            try
            {
                var existing = await _roleRepo.GetExistingRolePermissionsAsync(roleIds, permissionIds);

                var existingSet = new HashSet<(Guid RoleId, Guid PermissionId)>(
                    existing.Select(x => (x.RoleId, x.PermissionId))
                );

                var toAdd = new List<RolePermission>();

                foreach (var rId in roleIds)
                {
                    foreach (var pId in permissionIds)
                    {
                        if (!existingSet.Contains((rId, pId)))
                        {
                            toAdd.Add(new RolePermission { RoleId = rId, PermissionId = pId });
                        }
                    }
                }

                if (toAdd.Any())
                {
                    await _roleRepo.AddRolePermissionsRangeAsync(toAdd);
                    await _uow.SaveChangesAsync();
                }

                return ApiResponse<bool>.SuccessResponse(true, $"Successfully assigned {toAdd.Count} new permissions.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AssignPermissionsToRoles error: {Message}", ex.Message);
                return ApiResponse<bool>.FailResponse("Error assigning permissions to roles");
            }
        }

        public async Task<ApiResponse<bool>> RemovePermissionFromRole(List<Guid> permissionIds, List<Guid> roleIds)
        {
            try
            {
                await _roleRepo.RemoveRolePermissionsRangeAsync(permissionIds, roleIds);

                await _uow.SaveChangesAsync();

                _logger.LogInformation("Removed permissions from roles successfully");
                return ApiResponse<bool>.SuccessResponse(true, "Permissions removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemovePermissionFromRole error: {Message}", ex.Message);
                return ApiResponse<bool>.FailResponse("Error removing permissions from roles");
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
