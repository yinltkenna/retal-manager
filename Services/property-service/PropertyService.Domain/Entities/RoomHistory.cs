namespace PropertyService.Domain.Entities
{
    public class RoomHistory : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime CheckInAt { get; set; }
        public DateTime? CheckOutAt { get; set; }
    }
}
