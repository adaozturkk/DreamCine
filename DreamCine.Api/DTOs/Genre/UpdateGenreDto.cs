using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Genre
{
    public class UpdateGenreDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Title must be in range 2 to 50 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
