using TenancyService.src.Application.DTOs.Requests.TenantIdentityDocument;
using TenancyService.src.Application.DTOs.Responses.TenantIdentityDocument;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
{
    public interface ITenantIdentityDocumentService
    {
        Task<ApiResponse<List<TenantIdentityDocumentResponse>>> GetByTenantIdAsync(Guid tenantId);
        Task<ApiResponse<TenantIdentityDocumentResponse>> GetByIdAsync(Guid id);
        Task<ApiResponse<TenantIdentityDocumentResponse>> CreateAsync(CreateTenantIdentityDocumentRequest request);
        Task<ApiResponse<TenantIdentityDocumentResponse>> UpdateAsync(Guid id, UpdateTenantIdentityDocumentRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
