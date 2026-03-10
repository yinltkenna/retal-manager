namespace PropertyService.src.Domain.Entities
{
    public class RoomPrice : BaseEntity
    {
        public Guid RoomTypeId { get; set; }
        public decimal Price { get; set; }
        public int PercentageDiscount { get; set; } // If > 0, this price is discounted by this percentage
        public bool IsActive { get; set; } // Indicates if this price is currently active
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
