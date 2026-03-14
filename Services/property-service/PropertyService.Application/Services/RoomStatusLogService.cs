using AutoMapper;
using EventContracts.Authorization.Permissions.PropertyService;
using PropertyService.Application.DTOs.Responses.RoomStatusLog;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class RoomStatusLogService(IUnitOfWork unitOfWork,
                                     IMapper mapper,
                                     IPermissionChecker permissionChecker) : IRoomStatusLogService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<List<RoomStatusLogResponse>>> GetByRoomIdAsync(Guid roomId)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomStatusLogPermissions.VIEW))
                return ApiResponse<List<RoomStatusLogResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<RoomStatusLog>().ListAsync(rsl => rsl.RoomId == roomId);
            return ApiResponse<List<RoomStatusLogResponse>>.SuccessResponse(_mapper.Map<List<RoomStatusLogResponse>>(list));
        }
    }
}
