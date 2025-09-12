using BAMK.Application.DTOs.TShirt;
using BAMK.Application.Services;
using BAMK.API.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ITShirtService _tShirtService;
        private readonly IProductMappingService _productMappingService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            ITShirtService tShirtService, 
            IProductMappingService productMappingService,
            ILogger<ProductsController> logger)
        {
            _tShirtService = tShirtService;
            _productMappingService = productMappingService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm ürünleri getirir (pagination ile)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string? search = null, [FromQuery] string? category = null)
        {
            try
            {
                var result = await _tShirtService.GetPagedAsync(page, limit, search, category);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Ürünler getirilirken hata oluştu", 400);
                }

                // Get total count for pagination
                var totalResult = await _tShirtService.GetAllAsync();
                if (!totalResult.IsSuccess)
                {
                    return ErrorResponse("Ürünler getirilirken hata oluştu", 400);
                }

                var total = totalResult.Value.Count();
                return Ok(_productMappingService.MapToFrontendFormatWithPagination(result.Value, page, limit, total));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürünler getirilirken hata oluştu");
                return ErrorResponse("Ürünler getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// ID'ye göre ürün getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _tShirtService.GetByIdAsync(id);
                if (!result.IsSuccess)
                {
                    if (result.Error?.Code == ErrorCode.NotFound)
                    {
                        return ErrorResponse("Ürün bulunamadı", 404);
                    }
                    return ErrorResponse("Ürün getirilirken hata oluştu", 400);
                }

                var product = result.Value;
                var productData = _productMappingService.MapToFrontendFormat(product);

                return SuccessResponse(productData, "Ürün başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün getirilirken hata oluştu");
                return ErrorResponse("Ürün getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Öne çıkan ürünleri getirir
        /// </summary>
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured([FromQuery] int limit = 8)
        {
            try
            {
                var result = await _tShirtService.GetFeaturedAsync(limit);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Öne çıkan ürünler getirilirken hata oluştu", 400);
                }

                var mappedProducts = _productMappingService.MapToFrontendFormat(result.Value);
                return SuccessResponse(mappedProducts, "Öne çıkan ürünler başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Öne çıkan ürünler getirilirken hata oluştu");
                return ErrorResponse("Öne çıkan ürünler getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Ürün arama
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            try
            {
                var result = await _tShirtService.SearchAsync(q);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Arama yapılırken hata oluştu", 400);
                }

                var products = result.Value.ToList();
                var total = products.Count;
                var paginatedProducts = products
                    .Skip((page - 1) * limit)
                    .Take(limit);

                return Ok(_productMappingService.MapToFrontendFormatWithPagination(paginatedProducts, page, limit, total));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Arama yapılırken hata oluştu");
                return ErrorResponse("Arama yapılırken hata oluştu", 500);
            }
        }
    }
}
