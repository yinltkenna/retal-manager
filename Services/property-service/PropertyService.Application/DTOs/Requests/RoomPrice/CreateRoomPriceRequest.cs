namespace PropertyService.Application.DTOs.Requests.RoomPrice
{
    public class CreateRoomPriceRequest
    {
        public Guid RoomTypeId { get; set; }
        public decimal Price { get; set; }
        public int PercentageDiscount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
