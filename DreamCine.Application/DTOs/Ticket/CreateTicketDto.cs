using DreamCine.Application.DTOs.Payment;

namespace DreamCine.Application.DTOs.Ticket
{
    public class CreateTicketDto
    {
        public int MovieSessionId { get; set; }
        public int SeatId { get; set; }
        public PaymentInfoDto PaymentInfo { get; set; }
    }
}
