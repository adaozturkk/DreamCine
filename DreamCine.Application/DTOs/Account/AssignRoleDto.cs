using DreamCine.Core.Enums;

namespace DreamCine.Application.DTOs.Account
{
    public class AssignRoleDto
    {
        public string Email { get; set; }
        public UserRoles Role { get; set; }
    }
}
