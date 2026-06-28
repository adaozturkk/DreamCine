namespace DreamCine.Application.DTOs.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public decimal? Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
