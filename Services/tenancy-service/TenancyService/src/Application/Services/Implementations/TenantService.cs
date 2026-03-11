using AutoMapper;
using EventContracts.Authorization.Permissions;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.src.Application.DTOs.Requests.Tenant;
using TenancyService.src.Application.DTOs.Responses.Tenant;
using TenancyService.src.Application.Interfaces;
using TenancyService.src.Application.Services.Interfaces;
using TenancyService.src.Domain.Entities;
using TenancyService.src.Infrastructure.Repositories.Interfaces;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Implementations
{
    public class TenantService(IUnitOfWork unitOfWork,
                              IMapper mapper,
                              IPermissionChecker permissionChecker) : ITenantService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<TenantResponse>>> GetAllAsync()
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantPermissions.VIEW))
                return ApiResponse<List<TenantResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Tenant>().ListAsync();
            return ApiResponse<List<TenantResponse>>.SuccessResponse(_mapper.Map<List<TenantResponse>>(list));
        }

        public async Task<ApiResponse<TenantResponse>> GetByIdAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantPermissions.VIEW))
                return ApiResponse<TenantResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Tenant>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<TenantResponse>.FailResponse("Tenant not found");

            return ApiResponse<TenantResponse>.SuccessResponse(_mapper.Map<TenantResponse>(entity));
        }

        public async Task<ApiResponse<TenantResponse>> CreateAsync(CreateTenantRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantPermissions.CREATE))
                return ApiResponse<TenantResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<Tenant>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Repository<Tenant>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<TenantResponse>.SuccessResponse(_mapper.Map<TenantResponse>(entity));
        }

        public async Task<ApiResponse<TenantResponse>> UpdateAsync(Guid id, UpdateTenantRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantPermissions.UPDATE))
                return ApiResponse<TenantResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Tenant>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<TenantResponse>.FailResponse("Tenant not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Tenant>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<TenantResponse>.SuccessResponse(_mapper.Map<TenantResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantPermissions.DELETE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Tenant>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Tenant not found");

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Tenant>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
