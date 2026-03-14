using IdentityService.Application.Common.TemplateResponses;
using IdentityService.Application.DTOs.Requests.User;
using IdentityService.Application.DTOs.Responses.User;

namespace IdentityService.Application.Interfaces
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
        Task<ApiResponse<string>> CreateUserAsync(CreateUserRequest request, Guid currentUserId);
        Task<ApiResponse<string>> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<ApiResponse<string>> DeleteUserAsync(Guid idToDel, Guid idRequest); // Soft delete
        Task<ApiResponse<string>> LockUserAsync(Guid id);
        Task<ApiResponse<string>> UnlockUserAsync(Guid id);
    }
}
