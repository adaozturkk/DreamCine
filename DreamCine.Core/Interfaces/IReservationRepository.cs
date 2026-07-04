using DreamCine.Core.Helpers;
using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<List<Reservation>> GetReservationsByUserIdAsync(string userId);
        Task<List<Reservation>> GetAllWithFilteringAsync(ReservationQueryObject query);
    }
}
