using DreamCine.Infrastructure.Data;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Infrastructure.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) : base(context) 
        {
        }

        public async Task<bool> DoGenresExistAsync(List<int> genreIds)
        {
            int uniqueIdCount = genreIds.Distinct().Count();
            int existingCount = await _context.Genres
                .Where(g => genreIds.Contains(g.Id))
                .CountAsync();

            return uniqueIdCount == existingCount;
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Genres.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != excludeId);
        }
    }
}
