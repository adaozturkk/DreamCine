namespace DreamCine.Core.Helpers
{
    public class SeatQueryObject : BaseQueryObject
    {
        public int? ScreenId { get; set; } = null;
        public string? SeatNumber { get; set; } = null;
    }
}
