using DreamCine.Api.Helpers;
using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<List<Seat>> GetAllWithFilteringAsync(SeatQueryObject query);
        Task<bool> SeatExistsInScreenAsync(string seatNumber, int screenId, int? excludeId = null);
    }
}
