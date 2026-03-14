using PropertyService.Application.DTOs.Responses.RoomStatusLog;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
{
    public interface IRoomStatusLogService
    {
        Task<ApiResponse<List<RoomStatusLogResponse>>> GetByRoomIdAsync(Guid roomId);
    }
}
