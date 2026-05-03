using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre> CreateAsync(Genre genreModel);
        Task<Genre?> UpdateAsync(int id, Genre genreModel);
        Task<Genre?> DeleteAsync(int id);
        Task<bool> DoGenresExistAsync(List<int> genreIds);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}
