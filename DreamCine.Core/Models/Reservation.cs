using DreamCine.Core.Enums;

namespace DreamCine.Core.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public int MovieSessionId { get; set; }
        public MovieSession MovieSession { get; set; } = null!;
        public DateTime BookingTime { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public required ICollection<Ticket> Tickets { get; set; }
    }
}
