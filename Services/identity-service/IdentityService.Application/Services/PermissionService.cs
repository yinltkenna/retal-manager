using AutoMapper;
using IdentityService.Application.Common.TemplateResponses;
using IdentityService.Application.DTOs.Requests.Permession;
using IdentityService.Application.DTOs.Responses.Permessions;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Services
{
    public class PermissionService(ILogger<PermissionService> logger,
                                   IPermissionRepository permRepo,
                                   IRoleRepository roleRepo,
                                   IUnitOfWork uow,
                                   IMapper mapper) : IPermissionService
    {
        private readonly IPermissionRepository _permRepo = permRepo;
        private readonly IRoleRepository _roleRepo = roleRepo;
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<PermissionService> _logger = logger;

        /// <summary>
        /// Get all permissions in the systems. 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<PermissionResponse>>> GetAllAsync()
        {
            try
            {
                var perms = await _permRepo.GetAllAsync();
                var resp = _mapper.Map<List<PermissionResponse>>(perms);
                if (resp == null)
                {
                    return ApiResponse<List<PermissionResponse>>.FailResponse("No permissions found");
                }
                return ApiResponse<List<PermissionResponse>>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllAsync error: {ex.Message}");
                return ApiResponse<List<PermissionResponse>>.FailResponse("Error retrieving permissions");
            }
        }

        public async Task<ApiResponse<PermissionResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var perm = await _permRepo.GetByIdAsync(id);
                if (perm == null) return ApiResponse<PermissionResponse>.FailResponse("Permission not found");
                var resp = _mapper.Map<PermissionResponse>(perm);
                if (resp == null)
                {
                    return ApiResponse<PermissionResponse>.FailResponse("Error mapping permission");
                }
                return ApiResponse<PermissionResponse>.SuccessResponse(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetByIdAsync error: {ex.Message}");
                return ApiResponse<PermissionResponse>.FailResponse("Error retrieving permission");
            }
        }

        public async Task<ApiResponse<string>> CreateAsync(CreatePermissionRequest request)
        {
            try
            {
                // maybe check code uniqueness
                var existing = await _permRepo.GetPermissionByCodeAsync(request.Code);
                if (existing != null)
                    return ApiResponse<string>.FailResponse("Permission code already exists");

                var perm = _mapper.Map<Permission>(request);
                perm.Id = Guid.NewGuid();
                await _permRepo.AddAsync(perm);
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Permission created");
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error creating permission");
            }
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdatePermissionRequest request)
        {
            try
            {
                var perm = await _permRepo.GetByIdAsync(request.Id);
                if (perm == null) return ApiResponse<string>.FailResponse("Permission not found");

                // map fields
                _mapper.Map(request, perm);
                _permRepo.Update(perm);
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Permission updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error updating permission");
            }
        }

        public async Task<ApiResponse<string>> AssignToRoleAsync(AssignPermissionsToRoleRequest request)
        {
            try
            {
                var existing = await _permRepo.GetExistingRolePermissionsAsync(request.RoleIds, request.PermissionIds);

                var existingSet = new HashSet<(Guid RoleId, Guid PermissionId)>(
                    existing.Select(x => (x.RoleId, x.PermissionId))
                );

                var toAdd = new List<RolePermission>();

                foreach (var roleId in request.RoleIds)
                {
                    foreach (var permId in request.PermissionIds)
                    {
                        if (!existingSet.Contains((roleId, permId)))
                        {
                            toAdd.Add(new RolePermission
                            {
                                RoleId = roleId,
                                PermissionId = permId
                            });
                        }
                    }
                }

                if (toAdd.Count != 0)
                {
                    await _permRepo.AddRolePermissionsRangeAsync(toAdd);
                    await _uow.SaveChangesAsync();
                }

                return ApiResponse<string>.SuccessResponse("Assigned successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AssignToRoleAsync error: {Message}", ex.Message);
                return ApiResponse<string>.FailResponse("Error assigning permissions");
            }
        }

        public async Task<ApiResponse<string>> RemoveFromRoleAsync(RemovePermissionsToRoleRequest request)
        {
            try
            {
                await _permRepo.RemoveRolePermissionsRangeAsync(request.RoleIds, request.PermissionIds);

                // Lưu thay đổi qua Unit of Work
                await _uow.SaveChangesAsync();

                return ApiResponse<string>.SuccessResponse("Removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoveFromRoleAsync error: {Message}", ex.Message);
                return ApiResponse<string>.FailResponse("Error removing permissions");
            }
        }
    }
}

