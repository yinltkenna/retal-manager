using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PropertyService.src.Application.DTOs.Requests.RoomImage;
using PropertyService.src.Application.DTOs.Responses.RoomImage;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Domain.Entities;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Implementations
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

            await _unitOfWork.Repository<RoomImage>().DeleteAsync(entity);
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
            await _unitOfWork.Repository<RoomImage>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomImageResponse>.SuccessResponse(_mapper.Map<RoomImageResponse>(entity));
        }

        private async Task SetPrimaryImageForRoom(Guid roomId, Guid? excludeImageId = null)
        {
            var existingPrimary = await _unitOfWork.Repository<RoomImage>()
                .Query()
                .Where(i => i.RoomId == roomId && i.IsPrimary && (excludeImageId == null || i.Id != excludeImageId))
                .ToListAsync();

            foreach (var item in existingPrimary)
            {
                item.IsPrimary = false;
                item.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<RoomImage>().UpdateAsync(item);
            }
        }
    }
}
