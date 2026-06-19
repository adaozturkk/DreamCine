using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Status> CreateAsync(Status statusModel)
        {
            await _context.Statuses.AddAsync(statusModel);
            await _context.SaveChangesAsync();

            return statusModel;
        }

        public async Task<Status?> DeleteAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
            {
                return null;
            }

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();

            return status;
        }

        public async Task<List<Status>> GetAllAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<Status?> GetByIdAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
            {
                return null;
            }

            return status;
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Statuses.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != excludeId);
        }

        public async Task<bool> StatusExistsAsync(int id)
        {
            return await _context.Statuses.AnyAsync(x => x.Id == id);
        }

        public async Task<Status?> UpdateAsync(int id, Status statusModel)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
            {
                return null;
            }

            status.Name = statusModel.Name;
            await _context.SaveChangesAsync();

            return status;
        }
    }
}
