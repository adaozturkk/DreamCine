using DreamCine.Api.Models;

namespace DreamCine.Api.Interfaces
{
    public interface IStatusRepository
    {
        Task<List<Status>> GetAllAsync();
        Task<Status?> GetByIdAsync(int id);
        Task<Status> CreateAsync(Status statusModel);
        Task<Status?> UpdateAsync(int id, Status statusModel);
        Task<Status?> DeleteAsync(int id);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}
