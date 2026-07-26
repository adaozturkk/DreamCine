using DreamCine.Application.Common;
using DreamCine.Application.DTOs.Account;

namespace DreamCine.Application.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<string>> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ServiceResult<AuthResponseDto>> RefreshTokenAsync(TokenDto tokenDto);
        Task<ServiceResult<string>> AssignRoleAsync(AssignRoleDto roleDto);
        Task<ServiceResult<string>> ForgotPasswordAsync(ForgotPasswordDto passwordDto);
        Task<ServiceResult<string>> ResetPasswordAsync(ResetPasswordDto passwordDto);
        Task<ServiceResult<string>> ChangePasswordAsync(ChangePasswordDto passwordDto, string userEmail);
    }
}
