namespace PropertyService.Application.DTOs.Requests.RoomType
{
    public class CreateRoomTypeRequest
    {
        public Guid BranchId { get; set; }
        public string TypeName { get; set; }
        public decimal BasePrice { get; set; }
        public string Areas { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsDiscounted { get; set; }
    }
}
