namespace PropertyService.src.Application.DTOs.Requests.RoomType
{
    public class UpdateRoomTypeRequest
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
