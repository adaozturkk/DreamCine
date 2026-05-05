namespace DreamCine.Api.Helpers
{
    public class MovieQueryObject
    {
        public string? Title { get; set; } = null;
        public int? GenreId { get; set; } = null;

        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
