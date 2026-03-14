using FluentValidation;
using PropertyService.Application.DTOs.Requests.Amenity;

namespace PropertyService.Application.Validator
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
