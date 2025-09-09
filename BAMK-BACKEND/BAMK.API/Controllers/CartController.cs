using BAMK.Application.DTOs.TShirt;
using BAMK.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BAMK.API.Controllers
{
    public class CartController : BaseController
    {
        private readonly ITShirtService _tShirtService;
        private readonly ILogger<CartController> _logger;

        public CartController(ITShirtService tShirtService, ILogger<CartController> logger)
        {
            _tShirtService = tShirtService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcının sepetini getirir
        /// </summary>
        [HttpGet]
        [Authorize]
        public IActionResult GetCart()
        {
            try
            {
                // Şimdilik boş sepet döndür, gerçek implementasyon için veritabanı gerekli
                var cartItems = new List<object>();
                
                return SuccessResponse(cartItems, "Sepet başarıyla getirildi");
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
                // Ürünün varlığını kontrol et
                var productResult = await _tShirtService.GetByIdAsync(int.Parse(request.ProductId));
                if (!productResult.IsSuccess)
                {
                    return ErrorResponse("Ürün bulunamadı", 404);
                }

                var product = productResult.Value;
                if (product.StockQuantity < request.Quantity)
                {
                    return ErrorResponse("Yeterli stok bulunmuyor", 400);
                }

                // Şimdilik başarılı response döndür
                var cartItem = new
                {
                    id = Guid.NewGuid().ToString(),
                    product = new
                    {
                        id = product.Id.ToString(),
                        name = product.Name,
                        description = product.Description,
                        price = product.Price,
                        images = !string.IsNullOrEmpty(product.ImageUrl) ? new[] { product.ImageUrl } : new[] { "https://via.placeholder.com/300x300?text=No+Image" }
                    },
                    quantity = request.Quantity,
                    addedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                return SuccessResponse(cartItem, "Ürün sepete eklendi");
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
        public IActionResult GetCartSummary()
        {
            try
            {
                // Şimdilik boş sepet özeti döndür
                var summary = new
                {
                    totalItems = 0,
                    totalPrice = 0,
                    items = new List<object>()
                };

                return SuccessResponse(summary, "Sepet özeti başarıyla getirildi");
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
        public IActionResult ClearCart()
        {
            try
            {
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
