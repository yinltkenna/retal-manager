using PropertyService.Application.DTOs.Requests.RoomTypeAmenity;
using PropertyService.Application.DTOs.Responses.Amenity;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
{
    public interface IRoomTypeAmenityService
    {
        Task<ApiResponse<bool>> AddAmenityAsync(Guid roomTypeId, CreateRoomTypeAmenityRequest request);
        Task<ApiResponse<bool>> RemoveAmenityAsync(Guid roomTypeId, Guid amenityId);
        Task<ApiResponse<List<AmenityResponse>>> GetAmenitiesByRoomTypeIdAsync(Guid roomTypeId);
    }
}
