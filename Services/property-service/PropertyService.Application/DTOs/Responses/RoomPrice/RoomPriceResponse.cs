namespace PropertyService.Application.DTOs.Responses.RoomPrice
{
    public class RoomPriceResponse
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public decimal Price { get; set; }
        public int PercentageDiscount { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
