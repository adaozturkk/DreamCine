using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Status
{
    public class CreateStatusDto
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Name must be in range 1 to 20 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
