namespace DreamCine.Core.Helpers
{
    public class TicketQuery : BaseQueryObject
    {
        public string? UserEmail { get; set; } = null;
        public int? MovieSessionId { get; set; } = null;
        public string? Status { get; set; } = null;
    }
}
