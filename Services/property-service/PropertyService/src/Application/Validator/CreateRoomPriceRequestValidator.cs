using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomPrice;

namespace PropertyService.src.Application.Validator
{
    public class CreateRoomPriceRequestValidator : AbstractValidator<CreateRoomPriceRequest>
    {
        public CreateRoomPriceRequestValidator()
        {
            RuleFor(x => x.RoomTypeId).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }
}
