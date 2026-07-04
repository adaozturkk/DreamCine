using DreamCine.Core.Enums;

namespace DreamCine.Core.Helpers
{
    public class ReservationQueryObject : BaseQueryObject
    {
        public ReservationStatus? Status { get; set; }
        public string? Email { get; set; }
        public int? MovieSessionId { get; set; }
    }
}
