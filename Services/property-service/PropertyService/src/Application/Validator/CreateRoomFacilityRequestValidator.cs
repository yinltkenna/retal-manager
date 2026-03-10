using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomFacility;
using PropertyService.src.Domain.Enums;

namespace PropertyService.src.Application.Validator
{
    public class CreateRoomFacilityRequestValidator : AbstractValidator<CreateRoomFacilityRequest>
    {
        public CreateRoomFacilityRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            
        }
    }
}
