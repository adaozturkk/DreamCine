using Microsoft.AspNetCore.Identity;

namespace DreamCine.Api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}
