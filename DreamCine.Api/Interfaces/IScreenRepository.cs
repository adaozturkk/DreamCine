using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IScreenRepository : IRepository<Screen>
    {
        Task<bool> ScreenNumberExistsAsync(int screenNumber, int? excludeId = null);
        Task<bool> ExistsAsync(int id);
    }
}
