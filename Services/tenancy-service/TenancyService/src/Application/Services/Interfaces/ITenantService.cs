using TenancyService.src.Application.DTOs.Requests.Tenant;
using TenancyService.src.Application.DTOs.Responses.Tenant;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
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
