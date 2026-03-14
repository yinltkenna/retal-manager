namespace PropertyService.Domain.Entities
{
    public class Room : BaseEntity
    {
        public Guid RoomTypeId { get; set; }
        public Guid BranchId { get; set; }
        public string RoomNumber { get; set; }
        public string Status { get; set; }
        public bool IsAvailable { get; set; }
    }
}
