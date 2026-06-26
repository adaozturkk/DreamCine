using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
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
