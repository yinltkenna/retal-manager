namespace PropertyService.Domain.Entities
{
    public class RoomStatusLog : BaseEntity
    {
        public Guid RoomId { get; set; }
        public string OldStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public Guid? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
