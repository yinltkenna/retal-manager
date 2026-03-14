using AutoMapper;
using PropertyService.Application.DTOs.Requests.RoomPrice;
using PropertyService.Application.DTOs.Responses.RoomPrice;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class RoomPriceService(IUnitOfWork unitOfWork,
                            IMapper mapper) : IRoomPriceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<RoomPriceResponse>> CreateAsync(CreateRoomPriceRequest request)
        {
            var entity = _mapper.Map<RoomPrice>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<RoomPrice>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomPriceResponse>.SuccessResponse(_mapper.Map<RoomPriceResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<RoomPrice>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Room price not found");

            _unitOfWork.Repository<RoomPrice>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<RoomPriceResponse>> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<RoomPrice>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomPriceResponse>.FailResponse("Room price not found");

            return ApiResponse<RoomPriceResponse>.SuccessResponse(_mapper.Map<RoomPriceResponse>(entity));
        }

        public async Task<ApiResponse<RoomPriceResponse>> GetCurrentPriceAsync(Guid roomTypeId)
        {
            var now = DateTime.UtcNow;

            var current = await _unitOfWork.Repository<RoomPrice>().ListAsync(
                predicate: rp => rp.RoomTypeId == roomTypeId 
                    && rp.StartDate <= now 
                    && rp.EndDate >= now,
                orderBy: q => q.OrderByDescending(rp => rp.StartDate)
                );
            
            if (current == null)
                return ApiResponse<RoomPriceResponse>.FailResponse("No active price found");

            return ApiResponse<RoomPriceResponse>.SuccessResponse(_mapper.Map<RoomPriceResponse>(current));
        }

        public async Task<ApiResponse<List<RoomPriceResponse>>> GetHistoryAsync(Guid roomTypeId)
        {
            var history = await _unitOfWork.Repository<RoomPrice>().ListAsync(
                predicate: rp => rp.RoomTypeId == roomTypeId,
                orderBy: q => q.OrderByDescending(rp => rp.StartDate) // Truyền logic sắp xếp vào đây
            );

            return ApiResponse<List<RoomPriceResponse>>.SuccessResponse(_mapper.Map<List<RoomPriceResponse>>(history));
        }

        public async Task<ApiResponse<RoomPriceResponse>> UpdateAsync(Guid id, UpdateRoomPriceRequest request)
        {
            var entity = await _unitOfWork.Repository<RoomPrice>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomPriceResponse>.FailResponse("Room price not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Repository<RoomPrice>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomPriceResponse>.SuccessResponse(_mapper.Map<RoomPriceResponse>(entity));
        }
    }
}
