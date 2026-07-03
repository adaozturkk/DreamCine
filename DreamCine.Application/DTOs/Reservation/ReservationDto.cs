namespace DreamCine.Application.DTOs.Reservation
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int MovieSessionId { get; set; }
        public DateTime BookingTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<ReservationTicketDto> Tickets { get; set; } = new List<ReservationTicketDto>();
    }
}
