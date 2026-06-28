using DreamCine.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace DreamCine.Core.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public int MovieSessionId { get; set; }
        public MovieSession MovieSession { get; set; } = null!;
        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;
        public DateTime BookingTime { get; set; }
        public decimal PurchasePrice { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Pending;
    }
}
