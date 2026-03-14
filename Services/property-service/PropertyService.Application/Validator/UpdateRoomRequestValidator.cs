using FluentValidation;
using PropertyService.Application.DTOs.Requests.Room;

namespace PropertyService.Application.Validator
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
