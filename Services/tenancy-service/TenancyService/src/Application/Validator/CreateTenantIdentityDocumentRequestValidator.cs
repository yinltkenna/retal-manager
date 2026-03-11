using FluentValidation;
using TenancyService.src.Application.DTOs.Requests.TenantIdentityDocument;

namespace TenancyService.src.Application.Validator
{
    public class CreateTenantIdentityDocumentRequestValidator : AbstractValidator<CreateTenantIdentityDocumentRequest>
    {
        public CreateTenantIdentityDocumentRequestValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.DocumentType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DocumentNumber).NotEmpty().MaximumLength(100);
            RuleFor(x => x.IssuedPlace).NotEmpty().MaximumLength(200);
            RuleFor(x => x.IssuedDate).LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
