namespace DreamCine.Application.DTOs.Reservation
{
    public class CreateReservationDto
    {
        public int MovieSessionId { get; set; }
        public List<int> SeatIds { get; set; } = new List<int>();
    }
}
