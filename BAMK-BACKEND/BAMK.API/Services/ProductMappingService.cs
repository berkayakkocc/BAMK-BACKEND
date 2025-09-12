using BAMK.Application.DTOs.TShirt;

namespace BAMK.API.Services
{
    public class ProductMappingService : IProductMappingService
    {
        public object MapToFrontendFormat(TShirtDto product)
        {
            return new
            {
                id = product.Id.ToString(),
                name = product.Name,
                description = product.Description,
                price = product.Price,
                originalPrice = product.Price * 1.2m, // %20 indirim simülasyonu
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
        }

        public IEnumerable<object> MapToFrontendFormat(IEnumerable<TShirtDto> products)
        {
            return products.Select(MapToFrontendFormat);
        }

        public object MapToFrontendFormatWithPagination(IEnumerable<TShirtDto> products, int page, int limit, int total)
        {
            var totalPages = (int)Math.Ceiling((double)total / limit);
            
            return new
            {
                data = MapToFrontendFormat(products),
                pagination = new
                {
                    page = page,
                    limit = limit,
                    total = total,
                    totalPages = totalPages
                },
                message = "Ürünler başarıyla getirildi",
                success = true
            };
        }
    }
}
