using FluentValidation;
using TenancyService.src.Application.DTOs.Requests.Contract;

namespace TenancyService.src.Application.Validator
{
    public class UpdateContractRequestValidator : AbstractValidator<UpdateContractRequest>
    {
        public UpdateContractRequestValidator()
        {
            RuleFor(x => x.ContractCode).NotEmpty().MaximumLength(100);
            RuleFor(x => x.RoomId).NotEmpty();
            RuleFor(x => x.OwnerTenantId).NotEmpty();
            RuleFor(x => x.SignedRoomNumber).NotEmpty().MaximumLength(100);
            RuleFor(x => x.BranchAddress).NotEmpty().MaximumLength(400);
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
            RuleFor(x => x.SignedRoomType).MaximumLength(200);
            RuleFor(x => x.PaymentCycle).MaximumLength(50);
            RuleFor(x => x.BillingDay).InclusiveBetween(1, 31);
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}
