using AutoMapper;
using EventContracts.Authorization.Permissions;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.src.Application.DTOs.Requests.Contract;
using TenancyService.src.Application.DTOs.Responses.Contract;
using TenancyService.src.Application.Interfaces;
using TenancyService.src.Application.Services.Interfaces;
using TenancyService.src.Domain.Entities;
using TenancyService.src.Infrastructure.Repositories.Interfaces;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Implementations
{
    public class ContractService(IUnitOfWork unitOfWork,
                                IMapper mapper,
                                IPermissionChecker permissionChecker) : IContractService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<ContractResponse>>> GetAllAsync()
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractPermissions.VIEW))
                return ApiResponse<List<ContractResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Contract>().ListAsync();
            return ApiResponse<List<ContractResponse>>.SuccessResponse(_mapper.Map<List<ContractResponse>>(list));
        }

        public async Task<ApiResponse<ContractResponse>> GetByIdAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractPermissions.VIEW))
                return ApiResponse<ContractResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Contract>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<ContractResponse>.FailResponse("Contract not found");

            return ApiResponse<ContractResponse>.SuccessResponse(_mapper.Map<ContractResponse>(entity));
        }

        public async Task<ApiResponse<ContractResponse>> CreateAsync(CreateContractRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractPermissions.CREATE))
                return ApiResponse<ContractResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<Contract>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Repository<Contract>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ContractResponse>.SuccessResponse(_mapper.Map<ContractResponse>(entity));
        }

        public async Task<ApiResponse<ContractResponse>> UpdateAsync(Guid id, UpdateContractRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractPermissions.UPDATE))
                return ApiResponse<ContractResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Contract>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<ContractResponse>.FailResponse("Contract not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Contract>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ContractResponse>.SuccessResponse(_mapper.Map<ContractResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractPermissions.DELETE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Contract>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Contract not found");

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Contract>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
