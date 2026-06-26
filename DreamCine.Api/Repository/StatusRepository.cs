using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        public StatusRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Statuses.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != excludeId);
        }

        public async Task<bool> StatusExistsAsync(int id)
        {
            return await _context.Statuses.AnyAsync(x => x.Id == id);
        }
    }
}
