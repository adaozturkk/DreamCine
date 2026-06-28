using DreamCine.Core.Models;

namespace DreamCine.Core.Interfaces
{
    public interface IStatusRepository : IRepository<Status>
    {
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
        Task<bool> StatusExistsAsync(int id);
    }
}
