using FluentValidation;
using TenancyService.Application.DTOs.Requests.Tenant;

namespace TenancyService.Application.Validator
{
    public class UpdateTenantRequestValidator : AbstractValidator<UpdateTenantRequest>
    {
        public UpdateTenantRequestValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.IdentityCard).NotEmpty().MaximumLength(50);
            RuleFor(x => x.PermanentAddress).NotEmpty().MaximumLength(400);
            RuleFor(x => x.AvatarFileId).MaximumLength(200);
        }
    }
}
