using FluentValidation;
using IdentityService.Application.DTOs.Requests.Authentication;

namespace IdentityService.Application.Validator
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(8)
                .WithMessage("New password must be at least 8 characters.")
                .Matches("[A-Z]")
                .Must((model, password) => !password.Contains(model.CurrentPassword))
                .WithMessage("New password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("New password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("New password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("New password must contain at least one special character.")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must be different from the current password.");

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Confirm password is required.")
                .Equal(x => x.NewPassword)
                .WithMessage("Confirm password must match the new password.");
        }
    }
}
