using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        [Precision(3, 1)]
        public decimal Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
