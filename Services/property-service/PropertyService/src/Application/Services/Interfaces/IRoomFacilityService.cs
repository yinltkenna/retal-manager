using PropertyService.src.Application.DTOs.Requests.RoomFacility;
using PropertyService.src.Application.DTOs.Responses.RoomFacility;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomFacilityService
    {
        Task<ApiResponse<RoomFacilityResponse>> CreateAsync(Guid roomId, CreateRoomFacilityRequest request);
        Task<ApiResponse<RoomFacilityResponse>> UpdateAsync(Guid id, UpdateRoomFacilityRequest request);
        Task<ApiResponse<List<RoomFacilityResponse>>> GetByRoomIdAsync(Guid roomId);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
