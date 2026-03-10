using FluentValidation;
using PropertyService.src.Application.DTOs.Requests.RoomImage;

namespace PropertyService.src.Application.Validator
{
    public class CreateRoomImageRequestValidator : AbstractValidator<CreateRoomImageRequest>
    {
        public CreateRoomImageRequestValidator()
        {
            RuleFor(x => x.FileId).NotEmpty();
        }
    }
}
