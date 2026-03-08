namespace PropertyService.src.Domain.Entities
{
    public class RoomPrice
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public decimal Price { get; set; }
        public int PercentageDiscount { get; set; } // If > 0, this price is discounted by this percentage
        public bool IsActive { get; set; } // Indicates if this price is currently active
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
