using DreamCine.Application.DTOs.Reservation;
using FluentValidation;

namespace DreamCine.Application.Validations.Reservation
{
    public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationDtoValidator()
        {
            RuleFor(x => x.MovieSessionId)
                .GreaterThan(0).WithMessage("Movie session id must be greater than 0.");

            RuleFor(x => x.SeatIds)
                .NotEmpty().WithMessage("A reservation must have at least one seat.");

            RuleForEach(x => x.SeatIds)
                .GreaterThan(0).WithMessage("Seat IDs must be greater than 0.");
        }
    }
}
