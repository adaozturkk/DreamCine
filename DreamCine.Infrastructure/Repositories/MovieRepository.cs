using DreamCine.Infrastructure.Data;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Movie>> GetAllWithFilteringAsync(MovieQueryObject query)
        {
            var movies = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                movies = movies.Where(x => x.Title.Contains(query.Title));
            }

            if (query.GenreId.HasValue)
            {
                movies = movies.Where(x => x.MovieGenres.Any(m => m.GenreId == query.GenreId));
            }

            switch (query.SortBy?.ToLower())
            {
                case "title":
                    movies = query.IsDescending ? movies.OrderByDescending(x => x.Title) : movies.OrderBy(x => x.Title);
                    break;
                case "releasedate":
                    movies = query.IsDescending ? movies.OrderByDescending(x => x.ReleaseDate) : movies.OrderBy(x => x.ReleaseDate);
                    break;
                case "rating":
                    movies = query.IsDescending ? movies.OrderByDescending(x => x.Rating) : movies.OrderBy(x => x.Rating);
                    break;
                default:
                    movies = query.IsDescending ? movies.OrderByDescending(x => x.Id) : movies.OrderBy(x => x.Id);
                    break;
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await movies
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.Status)
                .Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public override async Task<Movie?> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(x => x.MovieGenres)
                .ThenInclude(x => x.Genre).Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return null;
            }

            return movie;
        }

        public override async Task<Movie?> UpdateAsync(int id, Movie movieModel)
        {
            var movie = await _context.Movies
                .Include(x => x.MovieGenres).FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return null;
            }

            movie.MovieGenres.Clear();

            movie.Title = movieModel.Title;
            movie.Description = movieModel.Description;
            movie.Duration = movieModel.Duration;
            movie.Rating = movieModel.Rating;
            movie.ReleaseDate = movieModel.ReleaseDate;
            movie.MovieGenres = movieModel.MovieGenres;
            movie.StatusId = movieModel.StatusId;
            movie.PosterPath = movieModel.PosterPath;

            await _context.SaveChangesAsync();

            var updatedMovie = await _context.Movies
                .Include(x => x.MovieGenres)
                .ThenInclude(x => x.Genre)
                .FirstOrDefaultAsync(x => x.Id == id);

            return updatedMovie;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Movies.AnyAsync(m => m.Id == id);
        }
    }
}
