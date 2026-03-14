using PropertyService.Application.DTOs.Requests.RoomFacility;
using PropertyService.Application.DTOs.Responses.RoomFacility;
using PropertyService.Application.TemplateResponses;


namespace PropertyService.Application.Interfaces
{
    public interface IRoomFacilityService
    {
        Task<ApiResponse<RoomFacilityResponse>> CreateAsync(Guid roomId, CreateRoomFacilityRequest request);
        Task<ApiResponse<RoomFacilityResponse>> UpdateAsync(Guid id, UpdateRoomFacilityRequest request);
        Task<ApiResponse<List<RoomFacilityResponse>>> GetByRoomIdAsync(Guid roomId);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
