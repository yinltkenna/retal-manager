using PropertyService.Application.DTOs.Requests.Reservation;
using PropertyService.Application.DTOs.Responses.Reservation;
using PropertyService.Application.DTOs.Responses.Room;
using PropertyService.Application.TemplateResponses;


namespace PropertyService.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ApiResponse<ReservationResponse>> CreateAsync(CreateReservationRequest request);
        Task<ApiResponse<bool>> CancelAsync(Guid id);
        Task<ApiResponse<List<ReservationResponse>>> GetAllAsync();
        Task<ApiResponse<List<ReservationResponse>>> GetByRoomIdAsync(Guid roomId);
        Task<ApiResponse<List<ReservationResponse>>> GetByTenantIdAsync(Guid tenantId);
        Task<ApiResponse<List<RoomResponse>>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate);
    }
}
