using DreamCine.Application.DTOs.Ticket;
using FluentValidation;

namespace DreamCine.Application.Validations.Ticket
{
    public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
    {
        public CreateTicketDtoValidator()
        {
            RuleFor(x => x.MovieSessionId)
                .GreaterThan(0).WithMessage("Movie session id must be greater than 0.");
            RuleFor(x => x.SeatId)
                .GreaterThan(0).WithMessage("Seat id must be greater than 0.");
            RuleFor(x => x.PaymentInfo.CardHolderName)
                .NotEmpty().WithMessage("Card holder name cannot be empty.");
            RuleFor(x => x.PaymentInfo.CardNumber)
                .NotEmpty().WithMessage("Card number cannot be empty.")
                .Length(16).WithMessage("Card number must be 16 characters.");
            RuleFor(x => x.PaymentInfo.ExpiryMonth)
                .InclusiveBetween(1, 12).WithMessage("Expiry month must be between 1 to 12.");
            RuleFor(x => x.PaymentInfo.ExpiryYear)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Expiry year cannot be in past.");
            RuleFor(x => x.PaymentInfo.Cvv)
                .NotEmpty().WithMessage("CVV cannot be empty.")
                .Length(3).WithMessage("CVV must be 3 characters.");
        }
    }
}
