using BAMK.Application.DTOs.ProductDetail;
using BAMK.Core.Common;

namespace BAMK.Application.Services
{
    public interface IProductDetailService
    {
        Task<Result<IEnumerable<ProductDetailDto>>> GetAllAsync();
        Task<Result<ProductDetailDto?>> GetByIdAsync(int id);
        Task<Result<ProductDetailDto?>> GetByTShirtIdAsync(int tShirtId);
        Task<Result<ProductDetailDto>> CreateAsync(CreateProductDetailDto createProductDetailDto);
        Task<Result<ProductDetailDto>> UpdateAsync(int id, UpdateProductDetailDto updateProductDetailDto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<IEnumerable<ProductDetailDto>>> GetActiveAsync();
        Task<Result<bool>> UpdateStatusAsync(int id, bool isActive);
    }
}

