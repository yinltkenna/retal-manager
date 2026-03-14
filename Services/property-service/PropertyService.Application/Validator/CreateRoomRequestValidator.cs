using FluentValidation;
using PropertyService.Application.DTOs.Requests.Room;

namespace PropertyService.Application.Validator
{
    public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
    {
        public CreateRoomRequestValidator()
        {
            RuleFor(x => x.RoomTypeId).NotEmpty();
            RuleFor(x => x.BranchId).NotEmpty();
            RuleFor(x => x.RoomNumber).NotEmpty().MaximumLength(50);
        }
    }
}
