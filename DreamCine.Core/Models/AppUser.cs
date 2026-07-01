using Microsoft.AspNetCore.Identity;

namespace DreamCine.Core.Models
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
