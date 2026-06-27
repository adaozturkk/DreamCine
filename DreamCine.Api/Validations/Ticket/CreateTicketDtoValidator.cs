using DreamCine.Api.DTOs.Ticket;
using FluentValidation;

namespace DreamCine.Api.Validations.Ticket
{
    public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
    {
        public CreateTicketDtoValidator()
        {
            RuleFor(x => x.MovieSessionId)
                .GreaterThan(0).WithMessage("Movie session id must be greater than 0.");
            RuleFor(x => x.SeatId)
                .GreaterThan(0).WithMessage("Seat id must be greater than 0.");
        }
    }
}
