using PropertyService.Application.DTOs.Requests.Room;
using PropertyService.Application.DTOs.Responses.Room;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
{
    public interface IRoomService
    {
        Task<ApiResponse<RoomResponse>> CreateAsync(CreateRoomRequest request);
        Task<ApiResponse<RoomResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<List<RoomResponse>>> GetAllAsync();
        Task<ApiResponse<List<RoomResponse>>> GetByRoomTypeIdAsync(Guid roomTypeId);
        Task<ApiResponse<RoomResponse>> UpdateAsync(Guid id, UpdateRoomRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<ApiResponse<RoomResponse>> UpdateStatusAsync(Guid id, string status);
    }
}
