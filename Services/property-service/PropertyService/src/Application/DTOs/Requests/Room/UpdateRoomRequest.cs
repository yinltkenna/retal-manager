namespace PropertyService.src.Application.DTOs.Requests.Room
{
    public class UpdateRoomRequest
    {
        public Guid RoomTypeId { get; set; }
        public Guid BranchId { get; set; }
        public string RoomNumber { get; set; }
        public string Status { get; set; } = "available";
        public bool IsAvailable { get; set; }
    }
}
