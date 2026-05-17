using DreamCine.Api.DTOs.Screen;
using FluentValidation;

namespace DreamCine.Api.Validations.Screen
{
    public class CreateScreenDtoValidator : AbstractValidator<CreateScreenDto>
    {
        public CreateScreenDtoValidator()
        {
            RuleFor(x => x.ScreenNumber)
                .NotEmpty().WithMessage("Screen number can't be empty.")
                .InclusiveBetween(1, 30).WithMessage("Screen number must be between 1 and 30.");
            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("Capacity can't be empty.")
                .InclusiveBetween(10, 1000).WithMessage("Capacity must be between 10 and 1000.");
        }
    }
}
