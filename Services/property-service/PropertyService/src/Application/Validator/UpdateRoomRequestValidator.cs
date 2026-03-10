using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.Room;

namespace PropertyService.src.Application.Validator
{
    public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
    {
        public UpdateRoomRequestValidator()
        {
            RuleFor(x => x.RoomTypeId).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.RoomNumber).NotEmpty().MaximumLength(50);
        }
    }
}
