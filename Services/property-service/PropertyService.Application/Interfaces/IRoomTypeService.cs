using PropertyService.Application.DTOs.Requests.RoomType;
using PropertyService.Application.DTOs.Responses.RoomType;
using PropertyService.Application.TemplateResponses;

namespace PropertyService.Application.Interfaces
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
