using IdentityService.src.Application.DTOs.Requests.User;
using IdentityService.src.Application.DTOs.Responses.User;
using IdentityService.src.Web.Common.TemplateResponses;

namespace IdentityService.src.Application.Services.Interfaces
{
    public interface IUserService
    {
        // Query operations
        Task<ApiResponse<UserDetailResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<PagedResponse<UserListResponse>>> GetAllUsers(UserQueryRequest request);
        Task<ApiResponse<UserProfileResponse>> GetProfileAsync(Guid userId);

        // User operations
        Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);

        // Admin operations
        Task<ApiResponse<string>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<string>> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<ApiResponse<string>> DeleteUserAsync(Guid id); // Soft delete
        Task<ApiResponse<string>> LockUserAsync(Guid id);
        Task<ApiResponse<string>> UnlockUserAsync(Guid id);
    }
}
