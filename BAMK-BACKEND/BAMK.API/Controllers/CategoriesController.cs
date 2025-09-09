using BAMK.Application.DTOs.TShirt;
using BAMK.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ITShirtService _tShirtService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ITShirtService tShirtService, ILogger<CategoriesController> logger)
        {
            _tShirtService = tShirtService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm kategorileri getirir
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _tShirtService.GetAllAsync();
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Kategoriler getirilirken hata oluştu", 400);
                }

                // Renkleri kategoriler olarak döndür
                var categories = result.Value
                    .Where(p => !string.IsNullOrEmpty(p.Color))
                    .GroupBy(p => p.Color)
                    .Select(g => new
                    {
                        id = g.Key?.ToLower().Replace(" ", "-") ?? "genel",
                        name = g.Key ?? "Genel",
                        slug = g.Key?.ToLower().Replace(" ", "-") ?? "genel",
                        description = $"{g.Key} renkli ürünler",
                        image = "https://via.placeholder.com/300x200?text=" + g.Key,
                        parentId = (string?)null,
                        isActive = true,
                        createdAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    })
                    .ToList();

                // Genel kategori ekle
                if (!categories.Any(c => c.id == "genel"))
                {
                    categories.Insert(0, new
                    {
                        id = "genel",
                        name = "Genel",
                        slug = "genel",
                        description = "Tüm ürünler",
                        image = "https://via.placeholder.com/300x200?text=Genel",
                        parentId = (string?)null,
                        isActive = true,
                        createdAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    });
                }

                return SuccessResponse(categories, "Kategoriler başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriler getirilirken hata oluştu");
                return ErrorResponse("Kategoriler getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// ID'ye göre kategori getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _tShirtService.GetAllAsync();
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Kategori getirilirken hata oluştu", 400);
                }

                var category = result.Value
                    .Where(p => !string.IsNullOrEmpty(p.Color))
                    .GroupBy(p => p.Color)
                    .FirstOrDefault(g => g.Key?.ToLower().Replace(" ", "-") == id.ToLower());

                if (category == null)
                {
                    return ErrorResponse("Kategori bulunamadı", 404);
                }

                var categoryData = new
                {
                    id = category.Key?.ToLower().Replace(" ", "-") ?? "genel",
                    name = category.Key ?? "Genel",
                    slug = category.Key?.ToLower().Replace(" ", "-") ?? "genel",
                    description = $"{category.Key} renkli ürünler",
                    image = "https://via.placeholder.com/300x200?text=" + category.Key,
                    parentId = (string?)null,
                    isActive = true,
                    createdAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                return SuccessResponse(categoryData, "Kategori başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori getirilirken hata oluştu");
                return ErrorResponse("Kategori getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Slug'a göre kategori getirir
        /// </summary>
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            return await GetById(slug);
        }
    }
}
