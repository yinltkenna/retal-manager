using PropertyService.src.Application.DTOs.Requests.RoomTypeAmenity;
using PropertyService.src.Application.DTOs.Responses.Amenity;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomTypeAmenityService
    {
        Task<ApiResponse<bool>> AddAmenityAsync(Guid roomTypeId, CreateRoomTypeAmenityRequest request);
        Task<ApiResponse<bool>> RemoveAmenityAsync(Guid roomTypeId, Guid amenityId);
        Task<ApiResponse<List<AmenityResponse>>> GetAmenitiesByRoomTypeIdAsync(Guid roomTypeId);
    }
}
