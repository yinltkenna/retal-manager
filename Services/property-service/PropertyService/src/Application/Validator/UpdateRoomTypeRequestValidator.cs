using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomType;

namespace PropertyService.src.Application.Validator
{
    public class UpdateRoomTypeRequestValidator : AbstractValidator<UpdateRoomTypeRequest>
    {
        public UpdateRoomTypeRequestValidator()
        {
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.TypeName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
        }
    }
}
