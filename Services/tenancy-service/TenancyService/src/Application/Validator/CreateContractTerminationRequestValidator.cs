using FluentValidation;
using TenancyService.src.Application.DTOs.Requests.ContractTermination;

namespace TenancyService.src.Application.Validator
{
    public class CreateContractTerminationRequestValidator : AbstractValidator<CreateContractTerminationRequest>
    {
        public CreateContractTerminationRequestValidator()
        {
            RuleFor(x => x.TerminationDate).LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
            RuleFor(x => x.RefundDeposit).GreaterThanOrEqualTo(0);
        }
    }
}
