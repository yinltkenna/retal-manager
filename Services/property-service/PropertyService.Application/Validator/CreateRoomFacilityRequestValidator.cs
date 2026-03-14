using FluentValidation;
using PropertyService.Application.DTOs.Requests.RoomFacility;

namespace PropertyService.Application.Validator
{
    public class CreateRoomFacilityRequestValidator : AbstractValidator<CreateRoomFacilityRequest>
    {
        public CreateRoomFacilityRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            
        }
    }
}
