using BAMK.Application.DTOs.Auth;
using BAMK.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                var responseData = new
                {
                    user = new
                    {
                        id = result.Email, // Frontend'in beklediği format
                        email = result.Email,
                        name = $"{result.FirstName} {result.LastName}",
                        firstName = result.FirstName,
                        lastName = result.LastName,
                        role = "customer" // Varsayılan rol
                    },
                    token = result.Token,
                    expiresAt = result.ExpiresAt
                };
                
                return SuccessResponse(responseData, "Giriş başarılı");
            }
            catch (UnauthorizedAccessException)
            {
                return ErrorResponse("Geçersiz email veya şifre", 401);
            }
            catch (Exception ex)
            {
                return ErrorResponse("Giriş işlemi sırasında hata oluştu", 400);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                var responseData = new
                {
                    user = new
                    {
                        id = result.Email,
                        email = result.Email,
                        name = $"{result.FirstName} {result.LastName}",
                        firstName = result.FirstName,
                        lastName = result.LastName,
                        role = "customer"
                    },
                    token = result.Token,
                    expiresAt = result.ExpiresAt
                };
                
                return SuccessResponse(responseData, "Kayıt başarılı");
            }
            catch (Exception ex)
            {
                return ErrorResponse("Kayıt işlemi sırasında hata oluştu", 400);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                
                if (string.IsNullOrEmpty(token))
                {
                    return ErrorResponse("Token bulunamadı", 400);
                }

                var result = await _authService.LogoutAsync(token);
                
                if (result)
                {
                    return SuccessResponse("Çıkış başarılı");
                }
                else
                {
                    return ErrorResponse("Geçersiz token", 400);
                }
            }
            catch (Exception ex)
            {
                return ErrorResponse("Çıkış işlemi sırasında hata oluştu", 400);
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;
                var email = User.FindFirst("email")?.Value;
                var firstName = User.FindFirst("firstName")?.Value;
                var lastName = User.FindFirst("lastName")?.Value;

                var userInfo = new
                {
                    id = userId,
                    email = email,
                    name = $"{firstName} {lastName}",
                    firstName = firstName,
                    lastName = lastName,
                    role = "customer"
                };

                return SuccessResponse(userInfo, "Kullanıcı bilgileri getirildi");
            }
            catch (Exception ex)
            {
                return ErrorResponse("Kullanıcı bilgileri alınırken hata oluştu", 400);
            }
        }
    }
}
