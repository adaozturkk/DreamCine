namespace DreamCine.Core.Helpers
{
    public class MovieQueryObject : BaseQueryObject
    {
        public string? Title { get; set; } = null;
        public int? GenreId { get; set; } = null;
    }
}
