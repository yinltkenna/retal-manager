using AutoMapper;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractFile;
using TenancyService.Application.DTOs.Responses.ContractFile;
using TenancyService.Application.Interfaces;
using TenancyService.Domain.Entities;
using TenancyService.Domain.Interfaces;

namespace TenancyService.Application.Services
{
    public class ContractFileService(IUnitOfWork unitOfWork,
                                   IMapper mapper,
                                   IPermissionChecker permissionChecker) : IContractFileService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<ContractFileResponse>>> GetByContractIdAsync(Guid contractId)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractFilePermissions.VIEW))
                return ApiResponse<List<ContractFileResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<ContractFile>().ListAsync(cf => cf.ContractId == contractId);
            return ApiResponse<List<ContractFileResponse>>.SuccessResponse(_mapper.Map<List<ContractFileResponse>>(list));
        }

        public async Task<ApiResponse<ContractFileResponse>> AddAsync(AddContractFileRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractFilePermissions.CREATE))
                return ApiResponse<ContractFileResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<ContractFile>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;
            entity.UploadedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<ContractFile>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ContractFileResponse>.SuccessResponse(_mapper.Map<ContractFileResponse>(entity));
        }

        public async Task<ApiResponse<bool>> RemoveAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractFilePermissions.DELETE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<ContractFile>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Contract file not found");

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<ContractFile>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
