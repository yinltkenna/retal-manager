using FluentValidation;
using TenancyService.src.Application.DTOs.Requests.Tenant;

namespace TenancyService.src.Application.Validator
{
    public class CreateTenantRequestValidator : AbstractValidator<CreateTenantRequest>
    {
        public CreateTenantRequestValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.IdentityCard).NotEmpty().MaximumLength(50);
            RuleFor(x => x.PermanentAddress).NotEmpty().MaximumLength(400);
            RuleFor(x => x.AvatarFileId).MaximumLength(200);
        }
    }
}
