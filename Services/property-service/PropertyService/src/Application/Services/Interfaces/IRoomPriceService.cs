using PropertyService.src.Application.DTOs.Requests.RoomPrice;
using PropertyService.src.Application.DTOs.Responses.RoomPrice;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomPriceService
    {
        Task<ApiResponse<RoomPriceResponse>> CreateAsync(CreateRoomPriceRequest request);
        Task<ApiResponse<RoomPriceResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<RoomPriceResponse>> UpdateAsync(Guid id, UpdateRoomPriceRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<ApiResponse<RoomPriceResponse>> GetCurrentPriceAsync(Guid roomTypeId);
        Task<ApiResponse<List<RoomPriceResponse>>> GetHistoryAsync(Guid roomTypeId);
    }
}
