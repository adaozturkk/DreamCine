using DreamCine.Core.Enums;

namespace DreamCine.Core.Helpers
{
    public class UserTicketQuery : BaseQueryObject
    {
        public int? MovieSessionId { get; set; } = null;
        public int? ReservationId { get; set; } = null;
        public TicketStatus? Status { get; set; }
    }
}
