using DreamCine.Api.Data;
using DreamCine.Api.Helpers;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> CreateAsync(Seat seatModel)
        {
            await _context.Seats.AddAsync(seatModel);
            await _context.SaveChangesAsync();

            return seatModel;
        }

        public async Task<Seat?> DeleteAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return null;
            }

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return seat;
        }

        public async Task<List<Seat>> GetAllAsync(SeatQueryObject query)
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

        public async Task<Seat?> GetByIdAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return null;
            }

            return seat;
        }

        public async Task<bool> SeatExistsInScreenAsync(string seatNumber, int screenId, int? excludeId = null)
        {
            return await _context.Seats.AnyAsync(x => x.SeatNumber.ToLower() == seatNumber.ToLower() && x.ScreenId == screenId && x.Id != excludeId);
        }

        public async Task<Seat?> UpdateAsync(int id, Seat seatModel)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return null;
            }

            seat.SeatNumber = seatModel.SeatNumber;
            seat.ScreenId = seatModel.ScreenId;
            await _context.SaveChangesAsync();

            return seat;
        }
    }
}
