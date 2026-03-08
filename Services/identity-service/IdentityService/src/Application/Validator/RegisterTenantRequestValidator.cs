using FluentValidation;
using IdentityService.src.Application.DTOs.Requests.Authentication;

namespace IdentityService.src.Application.Validator
{
    public class RegisterTenantRequestValidator : AbstractValidator<RegisterTenantRequest>
    {
        public RegisterTenantRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3)
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("'{PropertyName}' must be at least 3 characters long and contain only letters and numbers");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("'{PropertyName}' is not valid");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^0[0-9]{9,10}$")
                .WithMessage("'{PropertyName}' must start with 0 and be 10-11 digits long.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("'{PropertyName}' is required.")
                .MinimumLength(8).WithMessage("'{PropertyName}' must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("'{PropertyName}' must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("'{PropertyName}' must contain at least one special character.");

        }
    }
}
