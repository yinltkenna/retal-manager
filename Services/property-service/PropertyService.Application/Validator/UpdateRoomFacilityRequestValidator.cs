using FluentValidation;
using PropertyService.Application.DTOs.Requests.RoomFacility;

namespace PropertyService.Application.Validator
{
    public class UpdateRoomFacilityRequestValidator : AbstractValidator<UpdateRoomFacilityRequest>
    {
        public UpdateRoomFacilityRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            
        }
    }
}
