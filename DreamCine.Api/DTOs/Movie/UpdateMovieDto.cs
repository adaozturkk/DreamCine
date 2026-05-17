namespace DreamCine.Api.DTOs.Movie
{
    public class UpdateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public decimal? Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}
