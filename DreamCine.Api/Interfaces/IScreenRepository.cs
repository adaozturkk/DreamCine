using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IScreenRepository
    {
        Task<List<Screen>> GetAllAsync();
        Task<Screen?> GetByIdAsync(int id);
        Task<Screen> CreateAsync(Screen screenModel);
        Task<Screen?> UpdateAsync(int id, Screen screenModel);
        Task<Screen?> DeleteAsync(int id);
        Task<bool> ScreenNumberExistsAsync(int screenNumber, int? excludeId = null);
    }
}
