using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<bool> IsSeatTakenAsync(int movieSessionId, int seatId);
        Task<List<Ticket>> GetTicketsByUserIdAsync(string userId);
    }
}
