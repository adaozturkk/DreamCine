using DreamCine.Core.Helpers;
using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<bool> IsSeatTakenAsync(int movieSessionId, int seatId);
        Task<List<Ticket>> GetTicketsByUserIdAsync(string userId);
        Task<List<Ticket>> GetAllAsync(TicketQuery query);
    }
}
