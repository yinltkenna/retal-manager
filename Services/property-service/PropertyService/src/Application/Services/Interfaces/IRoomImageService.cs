using PropertyService.src.Application.DTOs.Requests.RoomImage;
using PropertyService.src.Application.DTOs.Responses.RoomImage;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomImageService
    {
        Task<ApiResponse<RoomImageResponse>> CreateAsync(Guid roomId, CreateRoomImageRequest request);
        Task<ApiResponse<List<RoomImageResponse>>> GetByRoomIdAsync(Guid roomId);
        Task<ApiResponse<RoomImageResponse>> SetPrimaryAsync(Guid imageId);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
