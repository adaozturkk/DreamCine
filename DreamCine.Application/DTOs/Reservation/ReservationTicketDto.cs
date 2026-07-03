namespace DreamCine.Application.DTOs.Reservation
{
    public class ReservationTicketDto
    {
        public int Id { get; set; }
        public string? MovieTitle { get; set; }
        public DateTime SessionTime { get; set; }
        public int ScreenNumber { get; set; }
        public string? SeatNumber { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
