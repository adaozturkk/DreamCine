namespace DreamCine.Application.DTOs.Seat
{
    public class UpdateSeatDto
    {
        public int ScreenId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
    }
}
