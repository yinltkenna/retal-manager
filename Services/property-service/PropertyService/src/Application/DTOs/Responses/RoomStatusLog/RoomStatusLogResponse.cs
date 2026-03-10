namespace PropertyService.src.Application.DTOs.Responses.RoomStatusLog
{
    public class RoomStatusLogResponse
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string OldStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public Guid? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
