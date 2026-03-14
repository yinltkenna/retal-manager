namespace PropertyService.Application.DTOs.Responses.Reservation
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
