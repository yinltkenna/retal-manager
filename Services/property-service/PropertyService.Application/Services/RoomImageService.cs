using AutoMapper;
using PropertyService.Application.DTOs.Requests.RoomImage;
using PropertyService.Application.DTOs.Responses.RoomImage;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class RoomImageService(IUnitOfWork unitOfWork,
                                  IMapper mapper) : IRoomImageService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<RoomImageResponse>> CreateAsync(Guid roomId, CreateRoomImageRequest request)
        {
            var entity = _mapper.Map<RoomImage>(request);
            entity.Id = Guid.NewGuid();
            entity.RoomId = roomId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            if (request.IsPrimary)
            {
                await SetPrimaryImageForRoom(roomId);
            }

            await _unitOfWork.Repository<RoomImage>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomImageResponse>.SuccessResponse(_mapper.Map<RoomImageResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<RoomImage>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Room image not found");

            _unitOfWork.Repository<RoomImage>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<RoomImageResponse>>> GetByRoomIdAsync(Guid roomId)
        {
            var list = await _unitOfWork.Repository<RoomImage>().ListAsync(ri => ri.RoomId == roomId);
            return ApiResponse<List<RoomImageResponse>>.SuccessResponse(_mapper.Map<List<RoomImageResponse>>(list));
        }

        public async Task<ApiResponse<RoomImageResponse>> SetPrimaryAsync(Guid imageId)
        {
            var entity = await _unitOfWork.Repository<RoomImage>().GetByIdAsync(imageId);
            if (entity == null)
                return ApiResponse<RoomImageResponse>.FailResponse("Room image not found");

            await SetPrimaryImageForRoom(entity.RoomId, imageId);

            entity.IsPrimary = true;
            entity.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Repository<RoomImage>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomImageResponse>.SuccessResponse(_mapper.Map<RoomImageResponse>(entity));
        }

        private async Task SetPrimaryImageForRoom(Guid roomId, Guid? excludeImageId = null)
        {
            // Trong Service của Application
            var existingPrimary = await _unitOfWork.Repository<RoomImage>()
                .ListAsync(i => i.RoomId == roomId && i.IsPrimary && (excludeImageId == null || i.Id != excludeImageId));

            foreach (var item in existingPrimary)
            {
                item.IsPrimary = false;
                item.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Repository<RoomImage>().Update(item);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
