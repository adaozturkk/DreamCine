using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.DTOs.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public decimal? Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
