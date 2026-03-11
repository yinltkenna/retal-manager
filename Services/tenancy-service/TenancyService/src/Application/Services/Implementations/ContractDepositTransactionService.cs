using AutoMapper;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.src.Application.DTOs.Requests.ContractDepositTransaction;
using TenancyService.src.Application.DTOs.Responses.ContractDepositTransaction;
using TenancyService.src.Application.Interfaces;
using TenancyService.src.Application.Services.Interfaces;
using TenancyService.src.Domain.Entities;
using TenancyService.src.Infrastructure.Repositories.Interfaces;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Implementations
{
    public class ContractDepositTransactionService(IUnitOfWork unitOfWork,
                                                   IMapper mapper,
                                                   IPermissionChecker permissionChecker) : IContractDepositTransactionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<ContractDepositTransactionResponse>>> GetByContractIdAsync(Guid contractId)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantDepositTransactionPermission.VIEW))
                return ApiResponse<List<ContractDepositTransactionResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<ContractDepositTransaction>().ListAsync(ct => ct.ContractId == contractId);
            return ApiResponse<List<ContractDepositTransactionResponse>>.SuccessResponse(_mapper.Map<List<ContractDepositTransactionResponse>>(list));
        }

        public async Task<ApiResponse<ContractDepositTransactionResponse>> CreateAsync(Guid contractId, CreateContractDepositTransactionRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantDepositTransactionPermission.CREATE))
                return ApiResponse<ContractDepositTransactionResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<ContractDepositTransaction>(request);
            entity.Id = Guid.NewGuid();
            entity.ContractId = contractId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Repository<ContractDepositTransaction>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ContractDepositTransactionResponse>.SuccessResponse(_mapper.Map<ContractDepositTransactionResponse>(entity));
        }
    }
}
