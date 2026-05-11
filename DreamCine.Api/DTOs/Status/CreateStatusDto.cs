using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.DTOs.Status
{
    public class CreateStatusDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
