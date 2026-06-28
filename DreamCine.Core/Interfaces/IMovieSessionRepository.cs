using DreamCine.Core.Helpers;
using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface IMovieSessionRepository : IRepository<MovieSession>
    {
        Task<List<MovieSession>> GetAllAsync(MovieSessionQuery query);
    }
}
