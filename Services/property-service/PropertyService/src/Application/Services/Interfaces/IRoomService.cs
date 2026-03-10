using PropertyService.src.Application.DTOs.Requests.Room;
using PropertyService.src.Application.DTOs.Responses.Room;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
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
