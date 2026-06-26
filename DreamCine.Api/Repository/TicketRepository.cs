using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Seat)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Movie)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Screen)
                .ToListAsync();
        }

        public override async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Seat)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Movie)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Screen)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
