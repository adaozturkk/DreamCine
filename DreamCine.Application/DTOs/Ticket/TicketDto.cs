namespace DreamCine.Application.DTOs.Ticket
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string? UserEmail { get; set; }
        public int MovieSessionId { get; set; }
        public string? MovieTitle { get; set; }
        public DateTime SessionTime { get; set; }
        public int SeatId { get; set; }
        public int ScreenNumber { get; set; }
        public string? SeatNumber { get; set; }
        public int ReservationId { get; set; }
        public DateTime BookingTime { get; set; }
        public decimal PurchasePrice { get; set; }
        public string? Status { get; set; }
    }
}
