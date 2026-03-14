namespace PropertyService.Application.DTOs.Responses.RoomMaintenance
{
    public class RoomMaintenanceResponse
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid? RequestedBy { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
