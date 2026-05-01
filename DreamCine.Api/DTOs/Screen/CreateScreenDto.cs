using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Screen
{
    public class CreateScreenDto
    {
        [Required]
        [Range(1, 30, ErrorMessage = "Screen number must be between 1 and 30.")]
        public int ScreenNumber { get; set; }
        [Required]
        [Range(10, 1000, ErrorMessage = "Capacity must be between 10 and 1000.")]
        public int Capacity { get; set; }
    }
}
