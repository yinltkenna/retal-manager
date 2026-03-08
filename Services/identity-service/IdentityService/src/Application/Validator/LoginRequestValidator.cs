using FluentValidation;
using IdentityService.src.Application.DTOs.Requests.Authentication;

namespace IdentityService.src.Application.Validator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            // Username: required, 5-50 chars, alphanumeric + underscores only
            RuleFor(x => x.Username).NotEmpty().MinimumLength(5).MaximumLength(50).Matches(@"^[a-zA-Z0-9_]*$")
                .WithMessage("Username is not valid");

            // PasswordHash: required
            RuleFor(x => x.PasswordHash).NotEmpty()
                .WithMessage("PasswordHash must be not empty ");
        }
    }
}
