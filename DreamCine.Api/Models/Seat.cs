namespace DreamCine.Api.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = null!;
        public string SeatNumber { get; set; } = string.Empty;

    }
}
