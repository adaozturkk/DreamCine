using DreamCine.Core.Helpers;
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

        public override async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Tickets)
                    .ThenInclude(t => t.Seat)
                .Include(r => r.MovieSession)
                    .ThenInclude(ms => ms.Screen)
                .Include(r => r.MovieSession)
                    .ThenInclude(ms => ms.Movie)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Reservation>> GetAllWithFilteringAsync(ReservationQueryObject query)
        {
            var reservations = _context.Reservations
                .Include(r => r.Tickets)
                    .ThenInclude(t => t.Seat)
                .Include(r => r.MovieSession)
                    .ThenInclude(ms => ms.Screen)
                .Include(r => r.MovieSession)
                    .ThenInclude(ms => ms.Movie)
                .AsQueryable();

            if (query.Status.HasValue)
            {
                reservations = reservations.Where(r => r.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                reservations = reservations.Where(r => r.User.Email == query.Email);
            }

            if (query.MovieSessionId.HasValue)
            {
                reservations = reservations.Where(r => r.MovieSessionId == query.MovieSessionId);
            }

            switch (query.SortBy?.ToLower())
            {
                case "bookingtime":
                    reservations = query.IsDescending ? reservations.OrderByDescending(x => x.BookingTime) : reservations.OrderBy(x => x.BookingTime);
                    break;
                case "totalprice":
                    reservations = query.IsDescending ? reservations.OrderByDescending(x => x.TotalPrice) : reservations.OrderBy(x => x.TotalPrice);
                    break;
                default:
                    reservations = query.IsDescending ? reservations.OrderByDescending(x => x.Id) : reservations.OrderBy(x => x.Id);
                    break;
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await reservations.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }
    }
}
