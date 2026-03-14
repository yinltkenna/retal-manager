using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.Tenant;
using TenancyService.Application.DTOs.Responses.Tenant;

namespace TenancyService.Application.Interfaces
{
    public interface ITenantService
    {
        Task<ApiResponse<List<TenantResponse>>> GetAllAsync();
        Task<ApiResponse<TenantResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<TenantResponse>> CreateAsync(CreateTenantRequest request);
        Task<ApiResponse<TenantResponse>> UpdateAsync(Guid id, UpdateTenantRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
