using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> CreateAsync(Movie movieModel)
        {
            await _context.Movies.AddAsync(movieModel);
            await _context.SaveChangesAsync();

            return movieModel;
        }

        public async Task<Movie?> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return null;
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Movies.Include(x => x.MovieGenres).ThenInclude(x => x.Genre).ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return null;
            }

            return movie;
        }

        public async Task<Movie?> UpdateAsync(int id, Movie movieModel)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return null;
            }

            movie.Title = movieModel.Title;
            movie.Description = movieModel.Description;
            movie.Duration = movieModel.Duration;
            movie.Rating = movieModel.Rating;
            movie.ReleaseDate = movieModel.ReleaseDate;

            await _context.SaveChangesAsync();

            return movie;
        }
    }
}
