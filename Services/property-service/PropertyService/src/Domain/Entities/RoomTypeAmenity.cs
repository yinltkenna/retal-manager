namespace PropertyService.src.Domain.Entities
{
    public class RoomTypeAmenity : BaseEntity
    {
        public Guid RoomTypeId { get; set; }
        public Guid AmenityId { get; set; }
    }
}
