using DreamCine.Core.Helpers;
using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<List<Seat>> GetAllWithFilteringAsync(SeatQueryObject query);
        Task<bool> SeatExistsInScreenAsync(string seatNumber, int screenId, int? excludeId = null);
        Task<bool> ExistsAsync(int id);
        Task<List<Seat>> GetSeatsByIdAsync(List<int> seatIds);
    }
}
