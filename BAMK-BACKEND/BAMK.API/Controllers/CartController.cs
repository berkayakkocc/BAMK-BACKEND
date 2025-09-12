using BAMK.API.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcının sepetini getirir
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var result = await _cartService.GetCartAsync();
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
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var result = await _cartService.AddToCartAsync(request.ProductId, request.Quantity);
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
        /// Sepet özetini getirir
        /// </summary>
        [HttpGet("summary")]
        [Authorize]
        public async Task<IActionResult> GetCartSummary()
        {
            try
            {
                var result = await _cartService.GetCartSummaryAsync();
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
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var result = await _cartService.ClearCartAsync();
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

    public class AddToCartRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }
}
