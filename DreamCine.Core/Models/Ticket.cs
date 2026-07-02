using DreamCine.Core.Enums;

namespace DreamCine.Core.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;
        public decimal PurchasePrice { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Pending;
    }
}
