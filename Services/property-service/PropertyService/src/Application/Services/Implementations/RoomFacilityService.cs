using AutoMapper;
using EventContracts.Authorization.Permissions;
using PropertyService.src.Application.DTOs.Requests.RoomFacility;
using PropertyService.src.Application.DTOs.Responses.RoomFacility;
using PropertyService.src.Application.Interfaces;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Domain.Entities;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Implementations
{
    public class RoomFacilityService(IUnitOfWork unitOfWork,
                                    IMapper mapper,
                                    IPermissionChecker permissionChecker) : IRoomFacilityService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<RoomFacilityResponse>> CreateAsync(Guid roomId, CreateRoomFacilityRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomFacilityPermissions.CREATE))
                return ApiResponse<RoomFacilityResponse>.FailResponse("Unauthorized");

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(roomId);
            if (room == null)
                return ApiResponse<RoomFacilityResponse>.FailResponse("Room not found");

            var entity = _mapper.Map<RoomFacility>(request);
            entity.Id = Guid.NewGuid();
            entity.RoomId = roomId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<RoomFacility>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomFacilityResponse>.SuccessResponse(_mapper.Map<RoomFacilityResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomFacilityPermissions.DELETE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<RoomFacility>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Room facility not found");

            await _unitOfWork.Repository<RoomFacility>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<RoomFacilityResponse>>> GetByRoomIdAsync(Guid roomId)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomFacilityPermissions.VIEW))
                return ApiResponse<List<RoomFacilityResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<RoomFacility>().ListAsync(rf => rf.RoomId == roomId);
            return ApiResponse<List<RoomFacilityResponse>>.SuccessResponse(_mapper.Map<List<RoomFacilityResponse>>(list));
        }

        public async Task<ApiResponse<RoomFacilityResponse>> UpdateAsync(Guid id, UpdateRoomFacilityRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomFacilityPermissions.UPDATE))
                return ApiResponse<RoomFacilityResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<RoomFacility>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomFacilityResponse>.FailResponse("Room facility not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Repository<RoomFacility>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomFacilityResponse>.SuccessResponse(_mapper.Map<RoomFacilityResponse>(entity));
        }
    }
}
