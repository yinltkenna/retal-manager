using FluentValidation;
using TenancyService.src.Application.DTOs.Requests.ContractMember;

namespace TenancyService.src.Application.Validator
{
    public class AddContractMemberRequestValidator : AbstractValidator<AddContractMemberRequest>
    {
        public AddContractMemberRequestValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.TenantId).NotEmpty();
        }
    }
}
