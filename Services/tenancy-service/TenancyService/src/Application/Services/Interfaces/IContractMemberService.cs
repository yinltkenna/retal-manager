using TenancyService.src.Application.DTOs.Requests.ContractMember;
using TenancyService.src.Application.DTOs.Responses.ContractMember;
using TenancyService.src.Web.Common.TemplateResponses;

namespace TenancyService.src.Application.Services.Interfaces
{
    public interface IContractMemberService
    {
        Task<ApiResponse<List<ContractMemberResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractMemberResponse>> AddAsync(AddContractMemberRequest request);
        Task<ApiResponse<bool>> RemoveAsync(Guid id);
    }
}
