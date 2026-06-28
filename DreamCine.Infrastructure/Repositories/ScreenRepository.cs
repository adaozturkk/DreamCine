using DreamCine.Infrastructure.Data;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Infrastructure.Repositories
{
    public class ScreenRepository : Repository<Screen>, IScreenRepository
    {
        public ScreenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ScreenNumberExistsAsync(int screenNumber, int? excludeId = null)
        {
            return await _context.Screens.AnyAsync(x => x.ScreenNumber == screenNumber && x.Id != excludeId);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Screens.AnyAsync(x => x.Id == id);
        }
    }
}
