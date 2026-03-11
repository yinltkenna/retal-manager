using FluentValidation;
using TenancyService.src.Application.DTOs.Requests.TenantIdentityDocument;

namespace TenancyService.src.Application.Validator
{
    public class UpdateTenantIdentityDocumentRequestValidator : AbstractValidator<UpdateTenantIdentityDocumentRequest>
    {
        public UpdateTenantIdentityDocumentRequestValidator()
        {
            RuleFor(x => x.DocumentType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.DocumentNumber).NotEmpty().MaximumLength(100);
            RuleFor(x => x.IssuedPlace).NotEmpty().MaximumLength(200);
            RuleFor(x => x.IssuedDate).LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
