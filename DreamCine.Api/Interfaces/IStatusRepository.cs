using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IStatusRepository : IRepository<Status>
    {
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
        Task<bool> StatusExistsAsync(int id);
    }
}
