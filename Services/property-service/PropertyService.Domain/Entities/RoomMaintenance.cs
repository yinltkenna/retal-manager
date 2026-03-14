namespace PropertyService.Domain.Entities
{
    public class RoomMaintenance : BaseEntity
    {
        public Guid RoomId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid? RequestedBy { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
