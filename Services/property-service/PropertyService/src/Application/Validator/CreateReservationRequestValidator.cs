using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.Reservation;

namespace PropertyService.src.Application.Validator
{
    public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
    {
        public CreateReservationRequestValidator()
        {
            RuleFor(x => x.RoomId).NotEmpty();
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .LessThan(x => x.EndDate)
                .WithMessage("StartDate must be before EndDate");
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .GreaterThan(x => x.StartDate)
                .WithMessage("EndDate must be after StartDate");
        }
    }
}
