using FluentValidation;
using PropertyService.Application.DTOs.Requests.RoomMaintenance;

namespace PropertyService.Application.Validator
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
