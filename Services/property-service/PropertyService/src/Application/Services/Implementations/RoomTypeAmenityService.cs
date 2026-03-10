using AutoMapper;
using EventContracts.Authorization.Permissions;
using PropertyService.src.Application.DTOs.Requests.RoomTypeAmenity;
using PropertyService.src.Application.DTOs.Responses.Amenity;
using PropertyService.src.Application.Interfaces;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Domain.Entities;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Implementations
{
    public class RoomTypeAmenityService(IUnitOfWork unitOfWork,
                                       IMapper mapper,
                                       IPermissionChecker permissionChecker) : IRoomTypeAmenityService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<bool>> AddAmenityAsync(Guid roomTypeId, CreateRoomTypeAmenityRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomTypePermissions.UPDATE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(roomTypeId);
            if (roomType == null)
                return ApiResponse<bool>.FailResponse("Room type not found");

            var amenity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(request.AmenityId);
            if (amenity == null)
                return ApiResponse<bool>.FailResponse("Amenity not found");

            var existing = await _unitOfWork.Repository<RoomTypeAmenity>()
                .ListAsync(rt => rt.RoomTypeId == roomTypeId && rt.AmenityId == request.AmenityId);

            if (existing.Any())
                return ApiResponse<bool>.FailResponse("Amenity already added to room type");

            var entity = new RoomTypeAmenity
            {
                Id = Guid.NewGuid(),
                RoomTypeId = roomTypeId,
                AmenityId = request.AmenityId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<RoomTypeAmenity>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<AmenityResponse>>> GetAmenitiesByRoomTypeIdAsync(Guid roomTypeId)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomTypePermissions.VIEW))
                return ApiResponse<List<AmenityResponse>>.FailResponse("Unauthorized");

            var roomType = await _unitOfWork.Repository<RoomType>().GetByIdAsync(roomTypeId);
            if (roomType == null)
                return ApiResponse<List<AmenityResponse>>.FailResponse("Room type not found");

            var mappings = await _unitOfWork.Repository<RoomTypeAmenity>().ListAsync(rta => rta.RoomTypeId == roomTypeId);
            if (mappings == null || !mappings.Any())
                return ApiResponse<List<AmenityResponse>>.SuccessResponse(new List<AmenityResponse>());

            var amenityIds = mappings.Select(m => m.AmenityId).ToList();
            var amenities = await _unitOfWork.Repository<Amenity>().ListAsync(a => amenityIds.Contains(a.Id));

            var response = _mapper.Map<List<AmenityResponse>>(amenities);
            return ApiResponse<List<AmenityResponse>>.SuccessResponse(response);
        }

        public async Task<ApiResponse<bool>> RemoveAmenityAsync(Guid roomTypeId, Guid amenityId)
        {
            if (!await _permissionChecker.HasPermissionAsync(RoomTypePermissions.UPDATE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var mapping = (await _unitOfWork.Repository<RoomTypeAmenity>()
                .ListAsync(rta => rta.RoomTypeId == roomTypeId && rta.AmenityId == amenityId))
                .FirstOrDefault();

            if (mapping == null)
                return ApiResponse<bool>.FailResponse("Amenity mapping not found");

            await _unitOfWork.Repository<RoomTypeAmenity>().DeleteAsync(mapping);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}
