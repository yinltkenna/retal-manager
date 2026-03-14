namespace PropertyService.Application.DTOs.Requests.RoomMaintenance
{
    public class CreateRoomMaintenanceRequest
    {
        public Guid RoomId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid? RequestedBy { get; set; }
    }
}
