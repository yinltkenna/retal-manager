using PropertyService.src.Application.DTOs.Responses.RoomStatusLog;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomStatusLogService
    {
        Task<ApiResponse<List<RoomStatusLogResponse>>> GetByRoomIdAsync(Guid roomId);
    }
}
