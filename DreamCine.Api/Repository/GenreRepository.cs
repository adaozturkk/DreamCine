using DreamCine.Api.Data;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> CreateAsync(Genre genreModel)
        {
            await _context.Genres.AddAsync(genreModel);
            await _context.SaveChangesAsync();

            return genreModel;
        }

        public async Task<Genre?> DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return null;
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return genre;
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return null;
            }

            return genre;
        }

        public async Task<Genre?> UpdateAsync(int id, Genre genreModel)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return null;
            }

            genre.Name = genreModel.Name;
            await _context.SaveChangesAsync();

            return genre;
        }
    }
}
