using FluentValidation;
using PropertyService.Application.DTOs.Requests.RoomMaintenance;
using PropertyService.Domain.Enums;

namespace PropertyService.Application.Validator
{
    public class UpdateRoomMaintenanceRequestValidator : AbstractValidator<UpdateRoomMaintenanceRequest>
    {
        public UpdateRoomMaintenanceRequestValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(value => value == MaintenanceStatus.Pending || value == MaintenanceStatus.InProgress || value == MaintenanceStatus.Resolved)
                .WithMessage("Status must be a valid MaintenanceStatus value.");
        }
    }
}
