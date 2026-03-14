using AutoMapper;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractExtension;
using TenancyService.Application.DTOs.Responses.ContractExtension;
using TenancyService.Application.Interfaces;
using TenancyService.Domain.Entities;
using TenancyService.Domain.Interfaces;

namespace TenancyService.Application.Services
{
    public class ContractExtensionService(IUnitOfWork unitOfWork,
                                         IMapper mapper,
                                         IPermissionChecker permissionChecker) : IContractExtensionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<ContractExtensionResponse>>> GetByContractIdAsync(Guid contractId)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractExtensionPermissions.VIEW))
                return ApiResponse<List<ContractExtensionResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<ContractExtension>().ListAsync(ce => ce.ContractId == contractId);
            return ApiResponse<List<ContractExtensionResponse>>.SuccessResponse(_mapper.Map<List<ContractExtensionResponse>>(list));
        }

        public async Task<ApiResponse<ContractExtensionResponse>> CreateAsync(Guid contractId, CreateContractExtensionRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractExtensionPermissions.CREATE))
                return ApiResponse<ContractExtensionResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<ContractExtension>(request);
            entity.Id = Guid.NewGuid();
            entity.ContractId = contractId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Repository<ContractExtension>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ContractExtensionResponse>.SuccessResponse(_mapper.Map<ContractExtensionResponse>(entity));
        }
    }
}
