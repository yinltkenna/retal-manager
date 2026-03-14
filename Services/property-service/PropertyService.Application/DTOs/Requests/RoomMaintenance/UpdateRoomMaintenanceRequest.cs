namespace PropertyService.Application.DTOs.Requests.RoomMaintenance
{
    public class UpdateRoomMaintenanceRequest
    {
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
