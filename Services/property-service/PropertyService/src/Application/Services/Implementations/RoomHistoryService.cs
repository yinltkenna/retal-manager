using AutoMapper;
using EventContracts.Authorization.Permissions.PropertyService;
using PropertyService.src.Application.DTOs.Responses.RoomHistory;
using PropertyService.src.Application.Interfaces;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Implementations
{
    public class RoomHistoryService(IUnitOfWork unitOfWork,
                                   IMapper mapper,
                                   IPermissionChecker permissionChecker) : IRoomHistoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<RoomHistoryResponse>>> GetByRoomIdAsync(Guid roomId)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomHistoryPermissions.VIEW))
                return ApiResponse<List<RoomHistoryResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Domain.Entities.RoomHistory>().ListAsync(rh => rh.RoomId == roomId);
            return ApiResponse<List<RoomHistoryResponse>>.SuccessResponse(_mapper.Map<List<RoomHistoryResponse>>(list));
        }
    }
}
