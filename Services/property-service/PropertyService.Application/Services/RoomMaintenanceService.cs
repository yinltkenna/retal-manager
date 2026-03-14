using AutoMapper;
using EventContracts.Authorization.Permissions.PropertyService;
using PropertyService.Application.DTOs.Requests.RoomMaintenance;
using PropertyService.Application.DTOs.Responses.RoomMaintenance;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Enums;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class RoomMaintenanceService(IUnitOfWork unitOfWork,
                                       IMapper mapper,
                                       IPermissionChecker permissionChecker) : IRoomMaintenanceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<RoomMaintenanceResponse>> CreateAsync(CreateRoomMaintenanceRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(MaintenancePermissions.CREATE))
                return ApiResponse<RoomMaintenanceResponse>.FailResponse("Unauthorized");

            var room = await _unitOfWork.Repository<Domain.Entities.Room>().GetByIdAsync(request.RoomId);
            if (room == null)
                return ApiResponse<RoomMaintenanceResponse>.FailResponse("Room not found");

            var entity = _mapper.Map<Domain.Entities.RoomMaintenance>(request);
            entity.Id = Guid.NewGuid();
            entity.Status = MaintenanceStatus.Pending;
            entity.RequestedAt = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Domain.Entities.RoomMaintenance>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomMaintenanceResponse>.SuccessResponse(_mapper.Map<RoomMaintenanceResponse>(entity));
        }

        public async Task<ApiResponse<RoomMaintenanceResponse>> UpdateAsync(Guid id, UpdateRoomMaintenanceRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(MaintenancePermissions.UPDATE))
                return ApiResponse<RoomMaintenanceResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Domain.Entities.RoomMaintenance>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomMaintenanceResponse>.FailResponse("Maintenance request not found");

            entity.Description = request.Description;
            entity.Status = request.Status;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Domain.Entities.RoomMaintenance>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomMaintenanceResponse>.SuccessResponse(_mapper.Map<RoomMaintenanceResponse>(entity));
        }

        public async Task<ApiResponse<List<RoomMaintenanceResponse>>> GetAllAsync()
        {
            if (!await _permissionChecker.HasPermissionAsync(MaintenancePermissions.VIEW))
                return ApiResponse<List<RoomMaintenanceResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Domain.Entities.RoomMaintenance>().ListAsync();
            return ApiResponse<List<RoomMaintenanceResponse>>.SuccessResponse(_mapper.Map<List<RoomMaintenanceResponse>>(list));
        }
    }
}
