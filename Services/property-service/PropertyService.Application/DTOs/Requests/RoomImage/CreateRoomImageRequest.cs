namespace PropertyService.Application.DTOs.Requests.RoomImage
{
    public class CreateRoomImageRequest
    {
        public Guid FileId { get; set; }
        public bool IsPrimary { get; set; }
    }
}
