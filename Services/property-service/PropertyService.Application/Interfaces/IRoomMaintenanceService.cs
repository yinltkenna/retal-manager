using PropertyService.Application.DTOs.Requests.RoomMaintenance;
using PropertyService.Application.DTOs.Responses.RoomMaintenance;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
{
    public interface IRoomMaintenanceService
    {
        Task<ApiResponse<RoomMaintenanceResponse>> CreateAsync(CreateRoomMaintenanceRequest request);
        Task<ApiResponse<RoomMaintenanceResponse>> UpdateAsync(Guid id, UpdateRoomMaintenanceRequest request);
        Task<ApiResponse<List<RoomMaintenanceResponse>>> GetAllAsync();
    }
}
