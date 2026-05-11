namespace DreamCine.Api.Helpers
{
    public class SeatQueryObject : BaseQueryObject
    {
        public int? ScreenId { get; set; } = null;
        public string? SeatNumber { get; set; } = null;
    }
}
