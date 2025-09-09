using BAMK.Application.DTOs.Auth;
using BAMK.Application.DTOs.User;
using BAMK.Application.Services;
using BAMK.Core.Common;
using BCrypt.Net;

namespace BAMK.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IJwtService jwtService, IUserService userService, ILogger<AuthService> logger)
        {
            _jwtService = jwtService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Kullanıcıyı veritabanından bul
                var userResult = await _userService.GetByEmailAsync(loginDto.Email);
                if (!userResult.IsSuccess)
                {
                    throw new UnauthorizedAccessException("Geçersiz email veya şifre");
                }

                var user = userResult.Value;
                if (user == null || !user.IsActive)
                {
                    throw new UnauthorizedAccessException("Kullanıcı bulunamadı veya aktif değil");
                }

                // Şifre doğrulama
                var passwordResult = await _userService.ValidatePasswordAsync(loginDto.Email, loginDto.Password);
                if (!passwordResult.IsSuccess || !passwordResult.Value)
                {
                    throw new UnauthorizedAccessException("Geçersiz email veya şifre");
                }

                // JWT token oluştur
                var token = _jwtService.GenerateToken(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });

                return new AuthResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login işlemi sırasında hata oluştu");
                throw;
            }
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Kullanıcı oluşturma DTO'su
                var createUserDto = new CreateUserDto
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber
                };

                // UserService ile kullanıcı oluştur
                var userResult = await _userService.CreateAsync(createUserDto);
                if (!userResult.IsSuccess)
                {
                    throw new InvalidOperationException(userResult.Error?.Message ?? "Kullanıcı oluşturulamadı");
                }

                var user = userResult.Value;
                if (user == null)
                {
                    throw new InvalidOperationException("Kullanıcı oluşturulamadı");
                }

                // JWT token oluştur
                var token = _jwtService.GenerateToken(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });

                return new AuthResponseDto
                {
                    Token = token,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register işlemi sırasında hata oluştu");
                throw;
            }
        }

        public Task<bool> LogoutAsync(string token)
        {
            try
            {
                // JWT token'ı doğrula
                if (!_jwtService.ValidateToken(token))
                {
                    return Task.FromResult(false);
                }

                // Logout işlemi (şu an için sadece token doğrulaması)
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout işlemi sırasında hata oluştu");
                return Task.FromResult(false);
            }
        }
    }
}
