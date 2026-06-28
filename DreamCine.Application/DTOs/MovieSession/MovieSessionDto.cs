namespace DreamCine.Application.DTOs.MovieSession
{
    public class MovieSessionDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string? MovieName { get; set; }
        public int ScreenId { get; set; }
        public int? ScreenNumber { get; set; }
        public DateTime SessionTime { get; set; }
        public decimal Price { get; set; }
    }
}
