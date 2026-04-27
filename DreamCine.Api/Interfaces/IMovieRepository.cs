using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task<Movie> CreateAsync(Movie movieModel);
        Task<Movie?> UpdateAsync(int id, Movie movieModel);
        Task<Movie?> DeleteAsync(int id);
    }
}
