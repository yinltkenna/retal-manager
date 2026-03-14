using AutoMapper;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.TenantIdentityDocument;
using TenancyService.Application.DTOs.Responses.TenantIdentityDocument;
using TenancyService.Application.Interfaces;
using TenancyService.Domain.Entities;
using TenancyService.Domain.Interfaces;

namespace TenancyService.Application.Services
{
    public class TenantIdentityDocumentService(IUnitOfWork unitOfWork,
                                             IMapper mapper,
                                             IPermissionChecker permissionChecker) : ITenantIdentityDocumentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<TenantIdentityDocumentResponse>>> GetByTenantIdAsync(Guid tenantId)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantIdentityDocumentsPermissions.VIEW))
                return ApiResponse<List<TenantIdentityDocumentResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<TenantIdentityDocument>().ListAsync(d => d.TenantId == tenantId);
            return ApiResponse<List<TenantIdentityDocumentResponse>>.SuccessResponse(_mapper.Map<List<TenantIdentityDocumentResponse>>(list));
        }

        public async Task<ApiResponse<TenantIdentityDocumentResponse>> GetByIdAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantIdentityDocumentsPermissions.VIEW))
                return ApiResponse<TenantIdentityDocumentResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<TenantIdentityDocument>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<TenantIdentityDocumentResponse>.FailResponse("Tenant document not found");

            return ApiResponse<TenantIdentityDocumentResponse>.SuccessResponse(_mapper.Map<TenantIdentityDocumentResponse>(entity));
        }

        public async Task<ApiResponse<TenantIdentityDocumentResponse>> CreateAsync(CreateTenantIdentityDocumentRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantIdentityDocumentsPermissions.CREATE))
                return ApiResponse<TenantIdentityDocumentResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<TenantIdentityDocument>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Repository<TenantIdentityDocument>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<TenantIdentityDocumentResponse>.SuccessResponse(_mapper.Map<TenantIdentityDocumentResponse>(entity));
        }

        public async Task<ApiResponse<TenantIdentityDocumentResponse>> UpdateAsync(Guid id, UpdateTenantIdentityDocumentRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantIdentityDocumentsPermissions.UPDATE))
                return ApiResponse<TenantIdentityDocumentResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<TenantIdentityDocument>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<TenantIdentityDocumentResponse>.FailResponse("Tenant document not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<TenantIdentityDocument>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<TenantIdentityDocumentResponse>.SuccessResponse(_mapper.Map<TenantIdentityDocumentResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantIdentityDocumentsPermissions.DELETE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<TenantIdentityDocument>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Tenant document not found");

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<TenantIdentityDocument>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
