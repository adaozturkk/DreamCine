using DreamCine.Core.Models;

namespace DreamCine.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
