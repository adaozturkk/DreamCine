using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Screen> CreateAsync(Screen screenModel)
        {
            await _context.Screens.AddAsync(screenModel);
            await _context.SaveChangesAsync();

            return screenModel;
        }

        public async Task<Screen?> DeleteAsync(int id)
        {
            var screen = await _context.Screens.FindAsync(id);

            if (screen == null)
            {
                return null;
            }

            _context.Remove(screen);
            await _context.SaveChangesAsync();

            return screen;
        }

        public async Task<List<Screen>> GetAllAsync()
        {
            return await _context.Screens.ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(int id)
        {
            var screen = await _context.Screens.FindAsync(id);

            if (screen == null)
            {
                return null;
            }

            return screen;
        }

        public async Task<Screen?> UpdateAsync(int id, Screen screenModel)
        {
            var screen = await _context.Screens.FindAsync(id);

            if (screen == null)
            {
                return null;
            }

            screen.ScreenNumber = screenModel.ScreenNumber;
            screen.Capacity = screenModel.Capacity;

            await _context.SaveChangesAsync();
            return screen;
        }
    }
}
