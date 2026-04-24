namespace DreamCine.Api.Models
{
    public class MovieSession
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = null!;
        public DateTime SessionTime { get; set; }
    }
}
