using DreamCine.Core.Helpers;
using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<List<Movie>> GetAllWithFilteringAsync(MovieQueryObject query);
        Task<bool> ExistsAsync(int id);
    }
}
