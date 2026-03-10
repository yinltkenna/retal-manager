using PropertyService.src.Application.DTOs.Responses.RoomHistory;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomHistoryService
    {
        Task<ApiResponse<List<RoomHistoryResponse>>> GetByRoomIdAsync(Guid roomId);
    }
}
