using FluentValidation;
using PropertyService.Application.DTOs.Requests.RoomImage;

namespace PropertyService.Application.Validator
{
    public class CreateRoomImageRequestValidator : AbstractValidator<CreateRoomImageRequest>
    {
        public CreateRoomImageRequestValidator()
        {
            RuleFor(x => x.FileId).NotEmpty();
        }
    }
}
