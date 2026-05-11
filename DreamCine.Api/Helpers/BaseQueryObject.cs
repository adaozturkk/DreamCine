using System.ComponentModel.DataAnnotations;

namespace DreamCine.Api.Helpers
{
    public class BaseQueryObject
    {
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;
        [Range(1, 50, ErrorMessage = "Page size must be between 1 and 50.")]
        public int PageSize { get; set; } = 10;
    }
}
