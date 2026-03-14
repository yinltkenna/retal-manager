using AutoMapper;
using PropertyService.Application.DTOs.Requests.RoomType;
using PropertyService.Application.DTOs.Responses.RoomType;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class RoomTypeService(IUnitOfWork unitOfWork,
                                 IMapper mapper) : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<RoomTypeResponse>> CreateAsync(CreateRoomTypeRequest request)
        {
            var entity = _mapper.Map<RoomType>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<RoomType>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomTypeResponse>.SuccessResponse(_mapper.Map<RoomTypeResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<RoomType>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Room type not found");

            _unitOfWork.Repository<RoomType>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<RoomTypeResponse>>> GetByBranchIdAsync(Guid branchId)
        {
            var list = await _unitOfWork.Repository<RoomType>()
                                        .ListAsync(rt => rt.BranchId == branchId);
            return ApiResponse<List<RoomTypeResponse>>.SuccessResponse(_mapper.Map<List<RoomTypeResponse>>(list));
        }

        public async Task<ApiResponse<RoomTypeResponse>> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<RoomType>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomTypeResponse>.FailResponse("Room type not found");

            return ApiResponse<RoomTypeResponse>.SuccessResponse(_mapper.Map<RoomTypeResponse>(entity));
        }

        public async Task<ApiResponse<RoomTypeResponse>> UpdateAsync(Guid id, UpdateRoomTypeRequest request)
        {
            var entity = await _unitOfWork.Repository<RoomType>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<RoomTypeResponse>.FailResponse("Room type not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Repository<RoomType>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<RoomTypeResponse>.SuccessResponse(_mapper.Map<RoomTypeResponse>(entity));
        }
    }
}
