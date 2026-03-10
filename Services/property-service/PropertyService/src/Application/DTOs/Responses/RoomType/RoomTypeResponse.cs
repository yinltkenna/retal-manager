namespace PropertyService.src.Application.DTOs.Responses.RoomType
{
    public class RoomTypeResponse
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public string TypeName { get; set; }
        public decimal BasePrice { get; set; }
        public string Areas { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDiscounted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
