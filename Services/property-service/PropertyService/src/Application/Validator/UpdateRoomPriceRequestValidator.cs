using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomPrice;

namespace PropertyService.src.Application.Validator
{
    public class UpdateRoomPriceRequestValidator : AbstractValidator<UpdateRoomPriceRequest>
    {
        public UpdateRoomPriceRequestValidator()
        {
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }
}
