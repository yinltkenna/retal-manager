using FluentValidation;
using PropertyService.Application.DTOs.Requests.RoomTypeAmenity;

namespace PropertyService.Application.Validator
{
    public class CreateRoomTypeAmenityRequestValidator : AbstractValidator<CreateRoomTypeAmenityRequest>
    {
        public CreateRoomTypeAmenityRequestValidator()
        {
            RuleFor(x => x.AmenityId).NotEmpty();
        }
    }
}
