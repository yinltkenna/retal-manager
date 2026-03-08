namespace PropertyService.src.Domain.Entities
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string TypeName { get; set; }
        public decimal BasePrice { get; set; } 
        public string Areas { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
