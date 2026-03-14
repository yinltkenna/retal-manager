using FluentValidation;
using TenancyService.Application.DTOs.Requests.ContractExtension;

namespace TenancyService.Application.Validator
{
    public class CreateContractExtensionRequestValidator : AbstractValidator<CreateContractExtensionRequest>
    {
        public CreateContractExtensionRequestValidator()
        {
            RuleFor(x => x.OldEndDate).LessThan(x => x.NewEndDate);
            RuleFor(x => x.NewEndDate).GreaterThan(x => x.OldEndDate);
        }
    }
}
