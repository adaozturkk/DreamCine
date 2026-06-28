namespace DreamCine.Core.Helpers
{
    public class MovieSessionQuery : BaseQueryObject
    {
        public int? MovieId { get; set; } = null;
        public int? ScreenId { get; set; } = null;
        public DateTime? Date { get; set; } = null;
    }
}
