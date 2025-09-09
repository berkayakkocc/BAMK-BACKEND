using BAMK.Application.DTOs.TShirt;
using BAMK.Application.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ITShirtService _tShirtService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ITShirtService tShirtService, ILogger<ProductsController> logger)
        {
            _tShirtService = tShirtService;
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
                var result = await _tShirtService.GetAllAsync();
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Ürünler getirilirken hata oluştu", 400);
                }

                var products = result.Value.ToList();
                
                // Search filter
                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(p => 
                        p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (p.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                    ).ToList();
                }

                // Category filter (color as category for now)
                if (!string.IsNullOrEmpty(category))
                {
                    products = products.Where(p => 
                        p.Color?.Equals(category, StringComparison.OrdinalIgnoreCase) ?? false
                    ).ToList();
                }

                // Pagination
                var total = products.Count;
                var paginatedProducts = products
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Select(p => new
                    {
                        id = p.Id.ToString(),
                        name = p.Name,
                        description = p.Description,
                        price = p.Price,
                        originalPrice = p.Price * 1.2m, // %20 indirim simülasyonu
                        images = !string.IsNullOrEmpty(p.ImageUrl) ? new[] { p.ImageUrl } : new[] { "https://via.placeholder.com/300x300?text=No+Image" },
                        category = new
                        {
                            id = p.Color ?? "default",
                            name = p.Color ?? "Genel",
                            slug = (p.Color ?? "genel").ToLower().Replace(" ", "-")
                        },
                        stock = p.StockQuantity,
                        isActive = p.IsActive,
                        tags = new[] { p.Color ?? "genel", p.Size ?? "standart" },
                        createdAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        updatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    });

                return PaginatedResponse(paginatedProducts, page, limit, total, "Ürünler başarıyla getirildi");
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
                var productData = new
                {
                    id = product.Id.ToString(),
                    name = product.Name,
                    description = product.Description,
                    price = product.Price,
                    originalPrice = product.Price * 1.2m,
                    images = !string.IsNullOrEmpty(product.ImageUrl) ? new[] { product.ImageUrl } : new[] { "https://via.placeholder.com/300x300?text=No+Image" },
                    category = new
                    {
                        id = product.Color ?? "default",
                        name = product.Color ?? "Genel",
                        slug = (product.Color ?? "genel").ToLower().Replace(" ", "-")
                    },
                    stock = product.StockQuantity,
                    isActive = product.IsActive,
                    tags = new[] { product.Color ?? "genel", product.Size ?? "standart" },
                    createdAt = product.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    updatedAt = product.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

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
                var result = await _tShirtService.GetActiveAsync();
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Öne çıkan ürünler getirilirken hata oluştu", 400);
                }

                var featuredProducts = result.Value
                    .Take(limit)
                    .Select(p => new
                    {
                        id = p.Id.ToString(),
                        name = p.Name,
                        description = p.Description,
                        price = p.Price,
                        originalPrice = p.Price * 1.2m,
                        images = !string.IsNullOrEmpty(p.ImageUrl) ? new[] { p.ImageUrl } : new[] { "https://via.placeholder.com/300x300?text=No+Image" },
                        category = new
                        {
                            id = p.Color ?? "default",
                            name = p.Color ?? "Genel",
                            slug = (p.Color ?? "genel").ToLower().Replace(" ", "-")
                        },
                        stock = p.StockQuantity,
                        isActive = p.IsActive,
                        tags = new[] { p.Color ?? "genel", p.Size ?? "standart" },
                        createdAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        updatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    });

                return SuccessResponse(featuredProducts, "Öne çıkan ürünler başarıyla getirildi");
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
                var result = await _tShirtService.GetAllAsync();
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Arama yapılırken hata oluştu", 400);
                }

                var products = result.Value
                    .Where(p => 
                        p.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        (p.Description?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.Color?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
                    )
                    .ToList();

                var total = products.Count;
                var paginatedProducts = products
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Select(p => new
                    {
                        id = p.Id.ToString(),
                        name = p.Name,
                        description = p.Description,
                        price = p.Price,
                        originalPrice = p.Price * 1.2m,
                        images = !string.IsNullOrEmpty(p.ImageUrl) ? new[] { p.ImageUrl } : new[] { "https://via.placeholder.com/300x300?text=No+Image" },
                        category = new
                        {
                            id = p.Color ?? "default",
                            name = p.Color ?? "Genel",
                            slug = (p.Color ?? "genel").ToLower().Replace(" ", "-")
                        },
                        stock = p.StockQuantity,
                        isActive = p.IsActive,
                        tags = new[] { p.Color ?? "genel", p.Size ?? "standart" },
                        createdAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        updatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    });

                return PaginatedResponse(paginatedProducts, page, limit, total, "Arama sonuçları başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Arama yapılırken hata oluştu");
                return ErrorResponse("Arama yapılırken hata oluştu", 500);
            }
        }
    }
}
