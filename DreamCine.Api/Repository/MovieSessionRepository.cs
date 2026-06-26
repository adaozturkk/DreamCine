using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class MovieSessionRepository : Repository<MovieSession>, IMovieSessionRepository
    {
        public MovieSessionRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public override async Task<List<MovieSession>> GetAllAsync()
        {
            return await _context.MovieSessions
                .Include(x => x.Movie)
                .Include(x => x.Screen)
                .ToListAsync();
        }

        public override async Task<MovieSession?> GetByIdAsync(int id)
        {
            return await _context.MovieSessions
                .Include(x => x.Movie)
                .Include(x => x.Screen)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
