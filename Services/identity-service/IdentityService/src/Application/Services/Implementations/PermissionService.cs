using AutoMapper;
using IdentityService.src.Application.DTOs.Requests.Permession;
using IdentityService.src.Application.DTOs.Responses.Permessions;
using IdentityService.src.Application.Services.Interfaces;
using IdentityService.src.Domain.Entities;
using IdentityService.src.Infrastructure.Data;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using IdentityService.src.Web.Common.TemplateResponses;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.src.Application.Services.Implementations
{
    public class PermissionService(ILogger<PermissionService> logger,
                                   IPermissionRepository permRepo,
                                   IRoleRepository roleRepo,
                                   AppDbContext db,
                                   IUnitOfWork uow,
                                   IMapper mapper) : IPermissionService
    {
        private readonly IPermissionRepository _permRepo = permRepo;
        private readonly IRoleRepository _roleRepo = roleRepo;
        private readonly AppDbContext _db = db;
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
                var existing = await _db.Permissions.FirstOrDefaultAsync(p => p.Code == request.Code);
                if (existing != null)
                    return ApiResponse<string>.FailResponse("Permission code already exists");

                var perm = _mapper.Map<Permission>(request);
                perm.Id = Guid.NewGuid();
                await _db.Permissions.AddAsync(perm);
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
                foreach (var roleId in request.RoleIds)
                {
                    foreach (var permId in request.PermissionIds)
                    {
                        bool exists = await _db.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permId);
                        if (!exists)
                            _db.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permId });
                    }
                }
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Assigned");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AssignToRoleAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error assigning permissions");
            }
        }

        public async Task<ApiResponse<string>> RemoveFromRoleAsync(RemovePermissionsToRoleRequest request)
        {
            try
            {
                var toRemove = _db.RolePermissions.Where(rp => request.RoleIds.Contains(rp.RoleId) && request.PermissionIds.Contains(rp.PermissionId));
                _db.RolePermissions.RemoveRange(toRemove);
                await _uow.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Removed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"RemoveFromRoleAsync error: {ex.Message}");
                return ApiResponse<string>.FailResponse("Error removing permissions");
            }
        }
    }
}

