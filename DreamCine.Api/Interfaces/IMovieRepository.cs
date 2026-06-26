using DreamCine.Api.Helpers;
using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<List<Movie>> GetAllWithFilteringAsync(MovieQueryObject query);
        Task<bool> ExistsAsync(int id);
    }
}
