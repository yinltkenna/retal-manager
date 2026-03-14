namespace PropertyService.Application.DTOs.Responses.RoomImage
{
    public class RoomImageResponse
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid FileId { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
