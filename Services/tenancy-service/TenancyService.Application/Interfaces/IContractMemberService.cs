using TenancyService.Application.Common.TemplateResponses;
using TenancyService.Application.DTOs.Requests.ContractMember;
using TenancyService.Application.DTOs.Responses.ContractMember;

namespace TenancyService.Application.Interfaces
{
    public interface IContractMemberService
    {
        Task<ApiResponse<List<ContractMemberResponse>>> GetByContractIdAsync(Guid contractId);
        Task<ApiResponse<ContractMemberResponse>> AddAsync(AddContractMemberRequest request);
        Task<ApiResponse<bool>> RemoveAsync(Guid id);
    }
}
