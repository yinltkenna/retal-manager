using PropertyService.src.Application.DTOs.Requests.RoomMaintenance;
using PropertyService.src.Application.DTOs.Responses.RoomMaintenance;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomMaintenanceService
    {
        Task<ApiResponse<RoomMaintenanceResponse>> CreateAsync(CreateRoomMaintenanceRequest request);
        Task<ApiResponse<RoomMaintenanceResponse>> UpdateAsync(Guid id, UpdateRoomMaintenanceRequest request);
        Task<ApiResponse<List<RoomMaintenanceResponse>>> GetAllAsync();
    }
}
