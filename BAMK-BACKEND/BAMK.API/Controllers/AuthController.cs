using BAMK.Application.DTOs.Auth;
using BAMK.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Geçersiz email veya şifre" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Giriş işlemi sırasında hata oluştu", error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Kayıt işlemi sırasında hata oluştu", error = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Token bulunamadı" });
                }

                var result = await _authService.LogoutAsync(token);
                
                if (result)
                {
                    return Ok(new { message = "Çıkış başarılı" });
                }
                else
                {
                    return BadRequest(new { message = "Geçersiz token" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Çıkış işlemi sırasında hata oluştu", error = ex.Message });
            }
        }

        [HttpGet("me")]
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
                    Id = userId,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                return Ok(new { message = "Kullanıcı bilgileri getirildi", data = userInfo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Kullanıcı bilgileri alınırken hata oluştu", error = ex.Message });
            }
        }
    }
}
