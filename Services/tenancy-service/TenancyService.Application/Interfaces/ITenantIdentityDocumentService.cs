using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.TenantIdentityDocument;
using TenancyService.Application.DTOs.Responses.TenantIdentityDocument;

namespace TenancyService.Application.Interfaces
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
