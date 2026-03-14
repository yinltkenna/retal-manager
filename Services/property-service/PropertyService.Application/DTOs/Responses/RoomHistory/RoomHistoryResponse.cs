namespace PropertyService.Application.DTOs.Responses.RoomHistory
{
    public class RoomHistoryResponse
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CheckInAt { get; set; }
        public DateTime? CheckOutAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
