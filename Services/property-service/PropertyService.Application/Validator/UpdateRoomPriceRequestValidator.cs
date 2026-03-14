using FluentValidation;
using PropertyService.Application.DTOs.Requests.RoomPrice;

namespace PropertyService.Application.Validator
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
