using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<List<Reservation>> GetReservationsByUserIdAsync(string userId);
    }
}
