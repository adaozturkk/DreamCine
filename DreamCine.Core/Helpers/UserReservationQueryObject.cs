using DreamCine.Core.Enums;

namespace DreamCine.Core.Helpers
{
    public class UserReservationQueryObject : BaseQueryObject
    {
        public ReservationStatus? Status { get; set; }
        public int? MovieSessionId { get; set; }
    }
}
