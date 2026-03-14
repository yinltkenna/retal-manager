using PropertyService.Application.DTOs.Requests.Amenity;
using PropertyService.Application.DTOs.Responses.Amenity;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
{
    public interface IAmenityService
    {
        Task<ApiResponse<AmenityResponse>> CreateAsync(CreateAmenityRequest request);
        Task<ApiResponse<AmenityResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<List<AmenityResponse>>> GetAllAsync();
        Task<ApiResponse<AmenityResponse>> UpdateAsync(Guid id, UpdateAmenityRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
