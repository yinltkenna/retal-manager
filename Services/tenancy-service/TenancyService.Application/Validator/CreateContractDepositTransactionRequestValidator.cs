using FluentValidation;
using TenancyService.Application.DTOs.Requests.ContractDepositTransaction;
using TenancyService.Domain.Enums;

namespace TenancyService.Application.Validator
{
    public class CreateContractDepositTransactionRequestValidator : AbstractValidator<CreateContractDepositTransactionRequest>
    {
        public CreateContractDepositTransactionRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.TransactionType)
                .NotEmpty()
                .Must(type => DepositTransactionType.All.Contains(type))
                .WithMessage("TransactionType must be one of: deposit, refund, deduct.");
            RuleFor(x => x.TransactionDate).LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Note).MaximumLength(500);
        }
    }
}
