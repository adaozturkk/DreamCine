using DreamCine.Api.Helpers;
using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IMovieSessionRepository : IRepository<MovieSession>
    {
        Task<List<MovieSession>> GetAllAsync(MovieSessionQuery query);
    }
}
