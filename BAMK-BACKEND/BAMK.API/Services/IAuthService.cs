using BAMK.API.DTOs;

namespace BAMK.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> LogoutAsync(string token);
    }
}
