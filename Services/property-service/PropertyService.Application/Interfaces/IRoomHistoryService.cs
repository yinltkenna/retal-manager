using PropertyService.Application.DTOs.Responses.RoomHistory;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
{
    public interface IRoomHistoryService
    {
        Task<ApiResponse<List<RoomHistoryResponse>>> GetByRoomIdAsync(Guid roomId);
    }
}
