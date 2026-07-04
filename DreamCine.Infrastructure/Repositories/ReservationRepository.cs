using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using DreamCine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Infrastructure.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Tickets)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reservation>> GetReservationsByUserIdAsync(string userId)
        {
            return await _context.Reservations
                .Include(r => r.Tickets)
                    .ThenInclude(t => t.Seat)
                .Include(r => r.MovieSession)
                    .ThenInclude(ms => ms.Screen)
                .Include(r => r.MovieSession)
                    .ThenInclude(ms => ms.Movie)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
