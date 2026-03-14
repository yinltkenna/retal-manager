using FluentValidation;
using TenancyService.Application.DTOs.Requests.ContractFile;

namespace TenancyService.Application.Validator
{
    public class AddContractFileRequestValidator : AbstractValidator<AddContractFileRequest>
    {
        public AddContractFileRequestValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.FileId).NotEmpty();
            RuleFor(x => x.FileType).NotEmpty();
        }
    }
}
