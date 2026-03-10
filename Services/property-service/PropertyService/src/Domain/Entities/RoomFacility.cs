using PropertyService.src.Domain.Enums;

namespace PropertyService.src.Domain.Entities
{
    public class RoomFacility : BaseEntity
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = FacilityStatus.Good;
    }
}
