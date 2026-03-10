namespace PropertyService.src.Application.DTOs.Requests.RoomPrice
{
    public class UpdateRoomPriceRequest
    {
        public decimal Price { get; set; }
        public int PercentageDiscount { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
