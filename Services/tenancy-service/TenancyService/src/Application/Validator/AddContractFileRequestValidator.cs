using FluentValidation;
using TenancyService.src.Application.DTOs.Requests.ContractFile;

namespace TenancyService.src.Application.Validator
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
