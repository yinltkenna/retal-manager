namespace PropertyService.Application.DTOs.Requests.Amenity
{
    public class UpdateAmenityRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
