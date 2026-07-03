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
    }
}
