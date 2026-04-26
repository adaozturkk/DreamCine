using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Genre
{
    public class CreateGenreDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Genre name cannot be over 50 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
