using AutoMapper;
using EventContracts.Authorization.Permissions.PropertyService;
using PropertyService.Application.DTOs.Requests.Reservation;
using PropertyService.Application.DTOs.Responses.Reservation;
using PropertyService.Application.DTOs.Responses.Room;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Enums;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class ReservationService(IUnitOfWork unitOfWork,
                                   IMapper mapper,
                                   IPermissionChecker permissionChecker) : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<ReservationResponse>> CreateAsync(CreateReservationRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(ReservationPermissions.CREATE))
                return ApiResponse<ReservationResponse>.FailResponse("Unauthorized");

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(request.RoomId);
            if (room == null)
                return ApiResponse<ReservationResponse>.FailResponse("Room not found");

            // Check overlapping reservations
            var overlapping = await _unitOfWork.Repository<Reservation>()
                .ListAsync(r => r.RoomId == request.RoomId
                                && r.Status != ReservationStatus.Cancelled
                                && !(request.EndDate <= r.StartDate || request.StartDate >= r.EndDate));

            if (overlapping.Any())
                return ApiResponse<ReservationResponse>.FailResponse("Room is already reserved for the specified period");

            var entity = _mapper.Map<Reservation>(request);
            entity.Id = Guid.NewGuid();
            entity.Status = ReservationStatus.Confirmed;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Reservation>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ReservationResponse>.SuccessResponse(_mapper.Map<ReservationResponse>(entity));
        }

        public async Task<ApiResponse<bool>> CancelAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(ReservationPermissions.CANCEL))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Reservation not found");

            entity.Status = ReservationStatus.Cancelled;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Reservation>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<ReservationResponse>>> GetAllAsync()
        {
            if (!await _permissionChecker.HasPermissionAsync(ReservationPermissions.VIEW))
                return ApiResponse<List<ReservationResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Reservation>().ListAsync();
            return ApiResponse<List<ReservationResponse>>.SuccessResponse(_mapper.Map<List<ReservationResponse>>(list));
        }

        public async Task<ApiResponse<List<ReservationResponse>>> GetByRoomIdAsync(Guid roomId)
        {
            if (!await _permissionChecker.HasPermissionAsync(ReservationPermissions.VIEW))
                return ApiResponse<List<ReservationResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Reservation>().ListAsync(r => r.RoomId == roomId);
            return ApiResponse<List<ReservationResponse>>.SuccessResponse(_mapper.Map<List<ReservationResponse>>(list));
        }

        public async Task<ApiResponse<List<ReservationResponse>>> GetByTenantIdAsync(Guid tenantId)
        {
            if (!await _permissionChecker.HasPermissionAsync(ReservationPermissions.VIEW))
                return ApiResponse<List<ReservationResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Reservation>().ListAsync(r => r.TenantId == tenantId);
            return ApiResponse<List<ReservationResponse>>.SuccessResponse(_mapper.Map<List<ReservationResponse>>(list));
        }

        public async Task<ApiResponse<List<RoomResponse>>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate)
        {
            if (!await _permissionChecker.HasPermissionAsync(ReservationPermissions.VIEW))
                return ApiResponse<List<RoomResponse>>.FailResponse("Unauthorized");

            var reservedRoomIds = (await _unitOfWork.Repository<Reservation>()
                    .ListAsync(r => r.Status != ReservationStatus.Cancelled
                                    && !(endDate <= r.StartDate || startDate >= r.EndDate)))
                .Select(r => r.RoomId)
                .ToHashSet();

            var rooms = await _unitOfWork.Repository<Room>()
                .ListAsync(r => !reservedRoomIds.Contains(r.Id) && r.Status == RoomStatus.Available);

            return ApiResponse<List<RoomResponse>>.SuccessResponse(_mapper.Map<List<RoomResponse>>(rooms));
        }
    }
}
