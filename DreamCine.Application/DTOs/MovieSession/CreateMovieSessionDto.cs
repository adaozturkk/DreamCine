namespace DreamCine.Application.DTOs.MovieSession
{
    public class CreateMovieSessionDto
    {
        public int MovieId { get; set; }
        public int ScreenId { get; set; }
        public DateTime SessionTime { get; set; }
        public decimal Price { get; set; }
    }
}
