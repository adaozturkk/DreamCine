namespace DreamCine.Core.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PosterPath { get; set; }
        public int Duration { get; set; }
        public decimal? Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public int StatusId { get; set; }
        public Status? Status { get; set; }
    }
}
