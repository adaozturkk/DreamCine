using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<bool> DoGenresExistAsync(List<int> genreIds);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}
