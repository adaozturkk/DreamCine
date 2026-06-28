using Microsoft.AspNetCore.Identity;

namespace DreamCine.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(IdentityUser user);
    }
}
