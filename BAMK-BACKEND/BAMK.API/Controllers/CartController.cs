using BAMK.Application.DTOs.Cart;
using BAMK.Application.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Tüm cart işlemleri için authentication gerekli
    public class CartController : BaseController
    {
        private readonly BAMK.Application.Services.ICartService _cartService;

        public CartController(BAMK.Application.Services.ICartService cartService, ILogger<CartController> logger) : base(logger)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Kullanıcının sepetini getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] int userId)
        {
            try
            {
                var result = await _cartService.GetCartAsync(userId);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Sepet getirilirken hata oluştu", 400);
                }

                return SuccessResponse(result.Value, "Sepet başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet getirilirken hata oluştu");
                return ErrorResponse("Sepet getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sepete ürün ekler
        /// </summary>
        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto, [FromQuery] int userId)
        {
            try
            {
                var result = await _cartService.AddToCartAsync(userId, dto);
                if (!result.IsSuccess)
                {
                    if (result.Error?.Code == ErrorCode.NotFound)
                    {
                        return ErrorResponse("Ürün bulunamadı", 404);
                    }
                    return ErrorResponse(result.Error?.Message ?? "Ürün sepete eklenirken hata oluştu", 400);
                }

                return SuccessResponse(result.Value, "Ürün sepete eklendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün sepete eklenirken hata oluştu");
                return ErrorResponse("Ürün sepete eklenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sepet öğesini günceller
        /// </summary>
        [HttpPut("items")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto, [FromQuery] int userId)
        {
            try
            {
                var result = await _cartService.UpdateCartItemAsync(userId, dto);
                if (!result.IsSuccess)
                {
                    return ErrorResponse(result.Error?.Message ?? "Sepet öğesi güncellenirken hata oluştu", 400);
                }

                return SuccessResponse(result.Value, "Sepet öğesi güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet öğesi güncellenirken hata oluştu");
                return ErrorResponse("Sepet öğesi güncellenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sepetten ürün çıkarır
        /// </summary>
        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId, [FromQuery] int userId)
        {
            try
            {
                var result = await _cartService.RemoveFromCartAsync(userId, cartItemId);
                if (!result.IsSuccess)
                {
                    return ErrorResponse(result.Error?.Message ?? "Ürün sepetten çıkarılırken hata oluştu", 400);
                }

                return SuccessResponse("Ürün sepetten çıkarıldı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün sepetten çıkarılırken hata oluştu");
                return ErrorResponse("Ürün sepetten çıkarılırken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sepet özetini getirir
        /// </summary>
        [HttpGet("summary")]
        public async Task<IActionResult> GetCartSummary([FromQuery] int userId)
        {
            try
            {
                var result = await _cartService.GetCartSummaryAsync(userId);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Sepet özeti getirilirken hata oluştu", 400);
                }

                return SuccessResponse(result.Value, "Sepet özeti başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet özeti getirilirken hata oluştu");
                return ErrorResponse("Sepet özeti getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sepeti temizler
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> ClearCart([FromQuery] int userId)
        {
            try
            {
                var result = await _cartService.ClearCartAsync(userId);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Sepet temizlenirken hata oluştu", 400);
                }

                return SuccessResponse("Sepet başarıyla temizlendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet temizlenirken hata oluştu");
                return ErrorResponse("Sepet temizlenirken hata oluştu", 500);
            }
        }
    }
}