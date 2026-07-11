using DreamCine.Core.Enums;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using DreamCine.Infrastructure.Data;
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
            var tickets = GetTicketsWithIncludesQuery().AsQueryable();

            if (query.Status.HasValue)
            {
                tickets = tickets.Where(t => t.Status == query.Status.Value);
            }

            if (query.MovieSessionId.HasValue)
            {
                tickets = tickets.Where(t => t.Reservation.MovieSessionId == query.MovieSessionId);
            }

            if (query.ReservationId.HasValue)
            {
                tickets = tickets.Where(t => t.ReservationId == query.ReservationId);
            }

            if (!string.IsNullOrWhiteSpace(query.UserEmail))
            {
                tickets = tickets.Where(t => t.Reservation.User.Email == query.UserEmail);
            }

            switch (query.SortBy?.ToLower())
            {
                case "bookingtime":
                    tickets = query.IsDescending ? tickets.OrderByDescending(x => x.Reservation.BookingTime) : tickets.OrderBy(x => x.Reservation.BookingTime);
                    break;
                case "purchaseprice":
                    tickets = query.IsDescending ? tickets.OrderByDescending(x => x.PurchasePrice) : tickets.OrderBy(x => x.PurchasePrice);
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
            return await GetTicketsWithIncludesQuery().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> IsSeatTakenAsync(int movieSessionId, int seatId)
        {
            return await _context.Tickets.AnyAsync(t =>
                t.Reservation.MovieSessionId == movieSessionId &&
                t.SeatId == seatId &&
                t.Status != TicketStatus.Cancelled &&
                t.Status != TicketStatus.Refunded);
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, UserTicketQuery query)
        {
            var tickets = GetTicketsWithIncludesQuery()
                .Where(t => t.Reservation.UserId == userId)
                .AsQueryable();

            if (query.Status.HasValue)
            {
                tickets = tickets.Where(t => t.Status == query.Status.Value);
            }

            if (query.MovieSessionId.HasValue)
            {
                tickets = tickets.Where(t => t.Reservation.MovieSessionId == query.MovieSessionId);
            }

            if (query.ReservationId.HasValue)
            {
                tickets = tickets.Where(t => t.ReservationId == query.ReservationId);
            }

            switch (query.SortBy?.ToLower())
            {
                case "bookingtime":
                    tickets = query.IsDescending ? tickets.OrderByDescending(x => x.Reservation.BookingTime) : tickets.OrderBy(x => x.Reservation.BookingTime);
                    break;
                case "purchaseprice":
                    tickets = query.IsDescending ? tickets.OrderByDescending(x => x.PurchasePrice) : tickets.OrderBy(x => x.PurchasePrice);
                    break;
                default:
                    tickets = query.IsDescending ? tickets.OrderByDescending(x => x.Id) : tickets.OrderBy(x => x.Id);
                    break;
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await tickets.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<List<int>> GetOccupiedSeatIdsAsync(int movieSessionId)
        {
            var occupiedSeatIds = await _context.Tickets
                .Where(x => x.Reservation.MovieSessionId == movieSessionId &&
                    x.Reservation.Status != ReservationStatus.Cancelled &&
                    x.Reservation.Status != ReservationStatus.Refunded)
                .Select(t => t.SeatId)
                .ToListAsync();

            return occupiedSeatIds;
        }

        private IQueryable<Ticket> GetTicketsWithIncludesQuery()
        {
            return _context.Tickets
                .Include(t => t.Seat)
                .Include(t => t.Reservation)
                    .ThenInclude(r => r.User)
                .Include(t => t.Reservation)
                    .ThenInclude(r => r.MovieSession)
                        .ThenInclude(ms => ms.Movie)
                .Include(t => t.Reservation)
                    .ThenInclude(r => r.MovieSession)
                        .ThenInclude(ms => ms.Screen);
        }

    }
}
