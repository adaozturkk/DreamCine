using DreamCine.Api.Data;
using DreamCine.Api.Helpers;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Seat>> GetAllWithFilteringAsync(SeatQueryObject query)
        {
            var seats = _context.Seats.AsQueryable();

            if (!string.IsNullOrEmpty(query.SeatNumber))
            {
                seats = seats.Where(x => x.SeatNumber == query.SeatNumber);
            }

            if (query.ScreenId.HasValue)
            {
                seats = seats.Where(x => x.ScreenId == query.ScreenId);
            }

            switch (query.SortBy?.ToLower())
            {
                case "seatnumber":
                    seats = query.IsDescending ? seats.OrderByDescending(x => x.SeatNumber) : seats.OrderBy(x => x.SeatNumber);
                    break;
                default:
                    seats = query.IsDescending ? seats.OrderByDescending(x => x.Id) : seats.OrderBy(x => x.Id);
                    break;
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await seats.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<bool> SeatExistsInScreenAsync(string seatNumber, int screenId, int? excludeId = null)
        {
            return await _context.Seats.AnyAsync(x => x.SeatNumber.ToLower() == seatNumber.ToLower() && x.ScreenId == screenId && x.Id != excludeId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Seats.AnyAsync(x => x.Id == id);
        }
    }
}
