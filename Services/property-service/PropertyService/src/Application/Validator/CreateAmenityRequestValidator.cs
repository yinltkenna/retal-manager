using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.Amenity;

namespace PropertyService.src.Application.Validator
{
    public class CreateAmenityRequestValidator : AbstractValidator<CreateAmenityRequest>
    {
        public CreateAmenityRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
