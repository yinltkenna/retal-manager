namespace PropertyService.Domain.Entities
{
    public class RoomImage : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid FileId { get; set; }
        public bool IsPrimary { get; set; }
    }
}
