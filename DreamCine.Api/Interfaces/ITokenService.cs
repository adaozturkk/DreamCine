using Microsoft.AspNetCore.Identity;

namespace DreamCine.Api.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(IdentityUser user);
    }
}
