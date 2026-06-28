using DreamCine.Infrastructure.Data;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Infrastructure.Repositories
{
    public class MovieSessionRepository : Repository<MovieSession>, IMovieSessionRepository
    {
        public MovieSessionRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<List<MovieSession>> GetAllAsync(MovieSessionQuery query)
        {
            var sessions = _context.MovieSessions
                .Include(x => x.Movie)
                .Include(x => x.Screen)
                .AsQueryable();

            if (query.MovieId != null)
            {
                sessions = sessions.Where(s => s.MovieId == query.MovieId);
            }

            if (query.ScreenId != null)
            {
                sessions = sessions.Where(s => s.ScreenId == query.ScreenId);
            }

            if (query.Date != null)
            {
                sessions = sessions.Where(s => s.SessionTime.Date == query.Date);
            }

            switch (query.SortBy?.ToLower())
            {
                case "movietitle":
                    sessions = query.IsDescending ? sessions.OrderByDescending(x => x.Movie.Title) : sessions.OrderBy(x => x.Movie.Title);
                    break;
                case "screennumber":
                    sessions = query.IsDescending ? sessions.OrderByDescending(x => x.Screen.ScreenNumber) : sessions.OrderBy(x => x.Screen.ScreenNumber);
                    break;
                case "sessiontime":
                    sessions = query.IsDescending ? sessions.OrderByDescending(x => x.SessionTime) : sessions.OrderBy(x => x.SessionTime);
                    break;
                default:
                    sessions = query.IsDescending ? sessions.OrderByDescending(x => x.Id) : sessions.OrderBy(x => x.Id);
                    break;
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await sessions.Skip(skipNumber).Take(query.PageSize).ToListAsync();
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
