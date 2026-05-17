using DreamCine.Api.DTOs.Screen;
using FluentValidation;

namespace DreamCine.Api.Validations.Screen
{
    public class UpdateScreenDtoValidator : AbstractValidator<UpdateScreenDto>
    {
        public UpdateScreenDtoValidator()
        {
            RuleFor(x => x.ScreenNumber)
                .NotEmpty().WithMessage("Screen number can't be empty.")
                .GreaterThan(0).WithMessage("Screen number must be greater than 0.")
                .LessThan(31).WithMessage("Screen number must be less than 31.");
            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("Capacity can't be empty.")
                .GreaterThan(9).WithMessage("Capacity must be greater than 9.")
                .LessThan(1001).WithMessage("Capacity must be less than 1001.");
        }
    }
}
