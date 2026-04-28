using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Movie
{
    public class CreateMovieDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be in range 1 to 100 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be in range 10 to 500 characters")]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
        public int Duration { get; set; }
        public decimal? Rating { get; set; }
        [Required]
        [Range(typeof(DateTime), "1/1/1888", "1/1/2100", ErrorMessage = "Release date is invalid.")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}
