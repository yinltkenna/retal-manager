using AutoMapper;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractTermination;
using TenancyService.Application.DTOs.Responses.ContractTermination;
using TenancyService.Application.Interfaces;
using TenancyService.Domain.Entities;
using TenancyService.Domain.Interfaces;

namespace TenancyService.Application.Services
{
    public class ContractTerminationService(IUnitOfWork unitOfWork,
                                             IMapper mapper,
                                             IPermissionChecker permissionChecker) : IContractTerminationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<ContractTerminationResponse>>> GetByContractIdAsync(Guid contractId)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantTerminationPermissions.VIEW))
                return ApiResponse<List<ContractTerminationResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<ContractTermination>().ListAsync(ct => ct.ContractId == contractId);
            return ApiResponse<List<ContractTerminationResponse>>.SuccessResponse(_mapper.Map<List<ContractTerminationResponse>>(list));
        }

        public async Task<ApiResponse<ContractTerminationResponse>> CreateAsync(Guid contractId, CreateContractTerminationRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantTerminationPermissions.CREATE))
                return ApiResponse<ContractTerminationResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<ContractTermination>(request);
            entity.Id = Guid.NewGuid();
            entity.ContractId = contractId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Repository<ContractTermination>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ContractTerminationResponse>.SuccessResponse(_mapper.Map<ContractTerminationResponse>(entity));
        }
    }
}