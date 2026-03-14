using FluentValidation;
using TenancyService.Application.DTOs.Requests.ContractMember;

namespace TenancyService.Application.Validator
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
