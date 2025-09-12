using BAMK.Application.DTOs.TShirt;

namespace BAMK.API.Services
{
    public interface IProductMappingService
    {
        object MapToFrontendFormat(TShirtDto product);
        IEnumerable<object> MapToFrontendFormat(IEnumerable<TShirtDto> products);
        object MapToFrontendFormatWithPagination(IEnumerable<TShirtDto> products, int page, int limit, int total);
    }
}
