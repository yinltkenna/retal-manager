using PropertyService.Application.DTOs.Requests.RoomImage;
using PropertyService.Application.DTOs.Responses.RoomImage;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
{
    public interface IRoomImageService
    {
        Task<ApiResponse<RoomImageResponse>> CreateAsync(Guid roomId, CreateRoomImageRequest request);
        Task<ApiResponse<List<RoomImageResponse>>> GetByRoomIdAsync(Guid roomId);
        Task<ApiResponse<RoomImageResponse>> SetPrimaryAsync(Guid imageId);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
