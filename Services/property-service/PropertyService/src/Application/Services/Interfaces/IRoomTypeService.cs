using PropertyService.src.Application.DTOs.Requests.RoomType;
using PropertyService.src.Application.DTOs.Responses.RoomType;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Interfaces
{
    public interface IRoomTypeService
    {
        Task<ApiResponse<RoomTypeResponse>> CreateAsync(CreateRoomTypeRequest request);
        Task<ApiResponse<RoomTypeResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<List<RoomTypeResponse>>> GetByBranchIdAsync(Guid branchId);
        Task<ApiResponse<RoomTypeResponse>> UpdateAsync(Guid id, UpdateRoomTypeRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
