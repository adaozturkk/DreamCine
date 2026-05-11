using DreamCine.Api.DTOs.Seat;
using FluentValidation;

namespace DreamCine.Api.Validations.Seat
{
    public class CreateSeatDtoValidator : AbstractValidator<CreateSeatDto>
    {
        public CreateSeatDtoValidator()
        {
            RuleFor(x => x.ScreenId)
                .NotEmpty().WithMessage("Screen id cannot be empty.")
                .GreaterThan(0).WithMessage("Screen id must be greater than 0.");
            RuleFor(x => x.SeatNumber)
                .NotEmpty().WithMessage("Seat number cannot be empty.")
                .MaximumLength(10).WithMessage("Seat number cannot be longer than 10 characters.");
        }
    }
}
