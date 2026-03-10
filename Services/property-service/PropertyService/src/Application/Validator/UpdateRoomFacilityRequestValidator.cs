using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomFacility;
using PropertyService.src.Domain.Enums;

namespace PropertyService.src.Application.Validator
{
    public class UpdateRoomFacilityRequestValidator : AbstractValidator<UpdateRoomFacilityRequest>
    {
        public UpdateRoomFacilityRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            
        }
    }
}
