namespace PropertyService.src.Application.DTOs.Responses.Room
{
    public class RoomResponse
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public Guid BranchId { get; set; }
        public string RoomNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
