using AutoMapper;
using PropertyService.Application.DTOs.Requests.Room;
using PropertyService.Application.DTOs.Responses.Room;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class RoomService(IUnitOfWork unitOfWork,
                             IMapper mapper) : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<RoomResponse>> CreateAsync(CreateRoomRequest request)
        {
            var entity = _mapper.Map<Room>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Room>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomResponse>.SuccessResponse(_mapper.Map<RoomResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Room not found");

            _unitOfWork.Repository<Room>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<RoomResponse>>> GetAllAsync()
        {
            var list = await _unitOfWork.Repository<Room>().ListAsync();
            return ApiResponse<List<RoomResponse>>.SuccessResponse(_mapper.Map<List<RoomResponse>>(list));
        }

        public async Task<ApiResponse<List<RoomResponse>>> GetByRoomTypeIdAsync(Guid roomTypeId)
        {
            var list = await _unitOfWork.Repository<Room>().ListAsync(r => r.RoomTypeId == roomTypeId);
            return ApiResponse<List<RoomResponse>>.SuccessResponse(_mapper.Map<List<RoomResponse>>(list));
        }

        public async Task<ApiResponse<RoomResponse>> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomResponse>.FailResponse("Room not found");

            return ApiResponse<RoomResponse>.SuccessResponse(_mapper.Map<RoomResponse>(entity));
        }

        public async Task<ApiResponse<RoomResponse>> UpdateAsync(Guid id, UpdateRoomRequest request)
        {
            var entity = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomResponse>.FailResponse("Room not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Repository<Room>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomResponse>.SuccessResponse(_mapper.Map<RoomResponse>(entity));
        }

        public async Task<ApiResponse<RoomResponse>> UpdateStatusAsync(Guid id, string status)
        {
            var entity = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomResponse>.FailResponse("Room not found");
            if (string.IsNullOrWhiteSpace(status))
                return ApiResponse<RoomResponse>.FailResponse("Invalid status");

            var oldStatus = entity.Status;
            entity.Status = status;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Room>().Update(entity);

            var statusLog = new RoomStatusLog
            {
                Id = Guid.NewGuid(),
                RoomId = entity.Id,
                OldStatus = oldStatus,
                NewStatus = status,
                ChangedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Repository<RoomStatusLog>().AddAsync(statusLog);

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomResponse>.SuccessResponse(_mapper.Map<RoomResponse>(entity));
        }
    }
}
