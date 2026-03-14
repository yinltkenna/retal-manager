using AutoMapper;
using EventContracts.Authorization.Permissions.PropertyService;
using PropertyService.Application.DTOs.Responses.RoomHistory;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
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

            var list = await _unitOfWork.Repository<RoomHistory>()
                .ListAsync(rh => rh.RoomId == roomId);
            return ApiResponse<List<RoomHistoryResponse>>.SuccessResponse(_mapper.Map<List<RoomHistoryResponse>>(list));
        }
    }
}
