using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomMaintenance;

namespace PropertyService.src.Application.Validator
{
    public class CreateRoomMaintenanceRequestValidator : AbstractValidator<CreateRoomMaintenanceRequest>
    {
        public CreateRoomMaintenanceRequestValidator()
        {
            RuleFor(x => x.RoomId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        }
    }
}
