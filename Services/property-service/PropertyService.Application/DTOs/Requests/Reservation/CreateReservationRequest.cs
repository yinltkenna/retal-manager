namespace PropertyService.Application.DTOs.Requests.Reservation
{
    public class CreateReservationRequest
    {
        public Guid RoomId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
