using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomType;

namespace PropertyService.src.Application.Validator
{
    public class CreateRoomTypeRequestValidator : AbstractValidator<CreateRoomTypeRequest>
    {
        public CreateRoomTypeRequestValidator()
        {
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.TypeName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
        }
    }
}
