using AutoMapper;
using PropertyService.src.Application.DTOs.Requests.Room;
using PropertyService.src.Application.DTOs.Responses.Room;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Domain.Entities;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Implementations
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

            await _unitOfWork.Repository<Room>().DeleteAsync(entity);
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
            await _unitOfWork.Repository<Room>().UpdateAsync(entity);
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

            await _unitOfWork.Repository<Room>().UpdateAsync(entity);

            var statusLog = new Domain.Entities.RoomStatusLog
            {
                Id = Guid.NewGuid(),
                RoomId = entity.Id,
                OldStatus = oldStatus,
                NewStatus = status,
                ChangedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Repository<Domain.Entities.RoomStatusLog>().AddAsync(statusLog);

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomResponse>.SuccessResponse(_mapper.Map<RoomResponse>(entity));
        }
    }
}
