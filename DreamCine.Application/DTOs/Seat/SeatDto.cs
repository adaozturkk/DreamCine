namespace DreamCine.Application.DTOs.Seat
{
    public class SeatDto
    {
        public int Id { get; set; }
        public int ScreenId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
    }
}
