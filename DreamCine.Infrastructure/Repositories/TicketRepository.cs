using DreamCine.Infrastructure.Data;
using DreamCine.Core.Enums;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Infrastructure.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Ticket>> GetAllAsync(TicketQuery query)
        {
            var tickets = _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Seat)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Movie)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Screen)
                .AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                if (Enum.TryParse<TicketStatus>(query.Status, true, out var parsedStatus))
                {
                    tickets = tickets.Where(t => t.Status == parsedStatus);
                }
                else
                {
                    tickets = tickets.Where(t => false);
                }
            }

            if (query.MovieSessionId != null)
            {
                tickets = tickets.Where(t => t.MovieSessionId == query.MovieSessionId);
            }

            if (!string.IsNullOrWhiteSpace(query.UserEmail))
            {
                tickets = tickets.Where(t => t.User.Email == query.UserEmail);
            }

            switch (query.SortBy?.ToLower())
            {
                case "bookingtime":
                    tickets = query.IsDescending ? tickets.OrderByDescending(x => x.BookingTime) : tickets.OrderBy(x => x.BookingTime);
                    break;
                default:
                    tickets = query.IsDescending ? tickets.OrderByDescending(x => x.Id) : tickets.OrderBy(x => x.Id);
                    break;
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await tickets.Skip(skipNumber).Take(query.PageSize).ToListAsync();
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

        public async Task<bool> IsSeatTakenAsync(int movieSessionId, int seatId)
        {
            return await _context.Tickets.AnyAsync(t =>
                t.MovieSessionId == movieSessionId &&
                t.SeatId == seatId &&
                t.Status != TicketStatus.Cancelled &&
                t.Status != TicketStatus.Refunded);
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .Include(t => t.Seat)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Movie)
                .Include(t => t.MovieSession)
                    .ThenInclude(ms => ms.Screen)
                .ToListAsync();
        }
    }
}
