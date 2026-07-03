namespace DreamCine.Application.DTOs.MovieSession
{
    public class SessionSeatsDto
    {
        public int Capacity { get; set; }
        public List<int> OccupiedSeats {  get; set; } = new List<int>();
    }
}
