using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface IScreenRepository : IRepository<Screen>
    {
        Task<bool> ScreenNumberExistsAsync(int screenNumber, int? excludeId = null);
        Task<bool> ExistsAsync(int id);
    }
}
