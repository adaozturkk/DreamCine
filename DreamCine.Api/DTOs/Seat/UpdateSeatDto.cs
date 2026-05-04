using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Seat
{
    public class UpdateSeatDto
    {
        [Required]
        public int ScreenId { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Seat number must be between 1 and 10 characters.")]
        public string SeatNumber { get; set; } = string.Empty;
    }
}
