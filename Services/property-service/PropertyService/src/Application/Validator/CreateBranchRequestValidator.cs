using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.Branch;

namespace PropertyService.src.Application.Validator
{
    public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
    {
        public CreateBranchRequestValidator()
        {
            RuleFor(x => x.ManagerId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(400);
            RuleFor(x => x.Hotline).MaximumLength(50);
        }
    }
}
