using DreamCine.Api.DTOs.Payment;

namespace DreamCine.Api.DTOs.Ticket
{
    public class CreateTicketDto
    {
        public int MovieSessionId { get; set; }
        public int SeatId { get; set; }
        public PaymentInfoDto PaymentInfo { get; set; }
    }
}
