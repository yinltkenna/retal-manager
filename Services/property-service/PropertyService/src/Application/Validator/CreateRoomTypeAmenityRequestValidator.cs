using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomTypeAmenity;

namespace PropertyService.src.Application.Validator
{
    public class CreateRoomTypeAmenityRequestValidator : AbstractValidator<CreateRoomTypeAmenityRequest>
    {
        public CreateRoomTypeAmenityRequestValidator()
        {
            RuleFor(x => x.AmenityId).NotEmpty();
        }
    }
}
