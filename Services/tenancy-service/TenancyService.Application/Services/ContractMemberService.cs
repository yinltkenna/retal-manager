using AutoMapper;
using EventContracts.Authorization.Permissions;
using EventContracts.Authorization.Permissions.TenancyService;
using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractMember;
using TenancyService.Application.DTOs.Responses.ContractMember;
using TenancyService.Application.Interfaces;
using TenancyService.Domain.Entities;
using TenancyService.Domain.Interfaces;

namespace TenancyService.Application.Services
{
    public class ContractMemberService(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IPermissionChecker permissionChecker) : IContractMemberService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<ContractMemberResponse>>> GetByContractIdAsync(Guid contractId)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractMemberPermissions.VIEW))
                return ApiResponse<List<ContractMemberResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<ContractMember>().ListAsync(cm => cm.ContractId == contractId);
            return ApiResponse<List<ContractMemberResponse>>.SuccessResponse(_mapper.Map<List<ContractMemberResponse>>(list));
        }

        public async Task<ApiResponse<ContractMemberResponse>> AddAsync(AddContractMemberRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractMemberPermissions.CREATE))
                return ApiResponse<ContractMemberResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<ContractMember>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Repository<ContractMember>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ContractMemberResponse>.SuccessResponse(_mapper.Map<ContractMemberResponse>(entity));
        }

        public async Task<ApiResponse<bool>> RemoveAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(TenantContractMemberPermissions.DELETE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<ContractMember>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Contract member not found");

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<ContractMember>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}