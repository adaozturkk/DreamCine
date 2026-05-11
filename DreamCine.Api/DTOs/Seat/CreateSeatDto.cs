using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Seat
{
    public class CreateSeatDto
    {
        public int ScreenId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
    }
}
