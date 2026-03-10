namespace PropertyService.src.Domain.Entities
{
    public class RoomType : BaseEntity
    {
        public Guid BranchId { get; set; }
        public string TypeName { get; set; }
        public decimal BasePrice { get; set; }
        public string Areas { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDiscounted { get; set; }
    }
}
