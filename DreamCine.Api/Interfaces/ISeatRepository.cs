using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetAllAsync();
        Task<Seat?> GetByIdAsync(int id);
        Task<Seat> CreateAsync(Seat seatModel);
        Task<Seat?> UpdateAsync(int id, Seat seatModel);
        Task<Seat?> DeleteAsync(int id);
        Task<bool> SeatExistsInScreenAsync(string seatNumber, int screenId, int? excludeId = null);
    }
}
