namespace PropertyService.src.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid BrandId { get; set; }
        public string RoomNumber { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
