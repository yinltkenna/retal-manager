namespace PropertyService.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
