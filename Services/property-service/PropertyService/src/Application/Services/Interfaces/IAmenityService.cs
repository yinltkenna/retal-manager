using PropertyService.src.Application.DTOs.Requests.Amenity;
using PropertyService.src.Application.DTOs.Responses.Amenity;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
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
