using AutoMapper;
using EventContracts.Authorization.Permissions.PropertyService;
using PropertyService.src.Application.DTOs.Responses.RoomStatusLog;
using PropertyService.src.Application.Interfaces;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Implementations
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

            var list = await _unitOfWork.Repository<Domain.Entities.RoomStatusLog>().ListAsync(rsl => rsl.RoomId == roomId);
            return ApiResponse<List<RoomStatusLogResponse>>.SuccessResponse(_mapper.Map<List<RoomStatusLogResponse>>(list));
        }
    }
}
