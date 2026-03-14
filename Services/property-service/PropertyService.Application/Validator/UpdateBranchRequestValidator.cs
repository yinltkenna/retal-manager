using FluentValidation;
using PropertyService.Application.DTOs.Requests.Branch;

namespace PropertyService.Application.Validator
{
    public class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
    {
        public UpdateBranchRequestValidator()
        {
            RuleFor(x => x.ManagerId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(400);
            RuleFor(x => x.Hotline).MaximumLength(50);
        }
    }
}
