using AutoMapper;
using BAMK.Application.DTOs.ProductDetail;
using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using BAMK.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BAMK.Application.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IGenericRepository<ProductDetail> _productDetailRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductDetailService> _logger;

        public ProductDetailService(
            IGenericRepository<ProductDetail> productDetailRepository,
            IMapper mapper,
            ILogger<ProductDetailService> logger)
        {
            _productDetailRepository = productDetailRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<ProductDetailDto>>> GetAllAsync()
        {
            try
            {
                var productDetails = await _productDetailRepository.GetAllAsync();
                var productDetailDtos = _mapper.Map<IEnumerable<ProductDetailDto>>(productDetails);
                return Result<IEnumerable<ProductDetailDto>>.Success(productDetailDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm ürün detaylarını getirirken hata oluştu");
                return Result<IEnumerable<ProductDetailDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün detayları getirilemedi"));
            }
        }

        public async Task<Result<ProductDetailDto?>> GetByIdAsync(int id)
        {
            try
            {
                var productDetail = await _productDetailRepository.GetByIdAsync(id);
                if (productDetail == null)
                {
                    return Result<ProductDetailDto?>.Failure(Error.NotFound("ProductDetail.NotFound", $"ID {id} olan ürün detayı bulunamadı"));
                }

                var productDetailDto = _mapper.Map<ProductDetailDto>(productDetail);
                return Result<ProductDetailDto?>.Success(productDetailDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün detayı getirirken hata oluştu. ID: {Id}", id);
                return Result<ProductDetailDto?>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün detayı getirilemedi"));
            }
        }

        public async Task<Result<ProductDetailDto?>> GetByTShirtIdAsync(int tShirtId)
        {
            try
            {
                var productDetails = await _productDetailRepository.FindAsync(pd => pd.TShirtId == tShirtId);
                var productDetail = productDetails.FirstOrDefault();
                
                if (productDetail == null)
                {
                    return Result<ProductDetailDto?>.Failure(Error.NotFound("ProductDetail.NotFound", $"TShirt ID {tShirtId} için ürün detayı bulunamadı"));
                }

                var productDetailDto = _mapper.Map<ProductDetailDto>(productDetail);
                return Result<ProductDetailDto?>.Success(productDetailDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TShirt ID'ye göre ürün detayı getirirken hata oluştu. TShirt ID: {TShirtId}", tShirtId);
                return Result<ProductDetailDto?>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün detayı getirilemedi"));
            }
        }

        public async Task<Result<ProductDetailDto>> CreateAsync(CreateProductDetailDto createProductDetailDto)
        {
            try
            {
                _logger.LogInformation($"ProductDetail oluşturuluyor - TShirtId: {createProductDetailDto.TShirtId}");
                
                // Aynı TShirt için zaten ProductDetail var mı kontrol et
                var existingDetails = await _productDetailRepository.FindAsync(pd => pd.TShirtId == createProductDetailDto.TShirtId);
                _logger.LogInformation($"Mevcut ProductDetail sayısı: {existingDetails.Count()}");
                
                if (existingDetails.Any())
                {
                    _logger.LogWarning($"TShirtId {createProductDetailDto.TShirtId} için zaten ProductDetail mevcut");
                    return Result<ProductDetailDto>.Failure(Error.Create(ErrorCode.ValidationError, "Bu TShirt için zaten ürün detayı mevcut"));
                }

                _logger.LogInformation($"ProductDetail mapping yapılıyor...");
                var productDetail = _mapper.Map<Domain.Entities.ProductDetail>(createProductDetailDto);
                productDetail.CreatedAt = DateTime.UtcNow;
                productDetail.UpdatedAt = DateTime.UtcNow;
                
                _logger.LogInformation($"ProductDetail entity oluşturuldu - Id: {productDetail.Id}, TShirtId: {productDetail.TShirtId}");

                _logger.LogInformation($"ProductDetail repository'ye ekleniyor...");
                await _productDetailRepository.AddAsync(productDetail);
                
                _logger.LogInformation($"ProductDetail değişiklikleri kaydediliyor...");
                await _productDetailRepository.SaveChangesAsync();
                
                _logger.LogInformation($"ProductDetail başarıyla oluşturuldu - Id: {productDetail.Id}");

                var productDetailDto = _mapper.Map<ProductDetailDto>(productDetail);
                return Result<ProductDetailDto>.Success(productDetailDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ürün detayı oluştururken hata oluştu - TShirtId: {createProductDetailDto.TShirtId}");
                return Result<ProductDetailDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün detayı oluşturulamadı"));
            }
        }

        public async Task<Result<ProductDetailDto>> UpdateAsync(int id, UpdateProductDetailDto updateProductDetailDto)
        {
            try
            {
                var existingProductDetail = await _productDetailRepository.GetByIdAsync(id);
                if (existingProductDetail == null)
                {
                    return Result<ProductDetailDto>.Failure(Error.NotFound("ProductDetail.NotFound", $"ID {id} olan ürün detayı bulunamadı"));
                }

                _mapper.Map(updateProductDetailDto, existingProductDetail);
                existingProductDetail.UpdatedAt = DateTime.UtcNow;

                _productDetailRepository.Update(existingProductDetail);
                await _productDetailRepository.SaveChangesAsync();

                var productDetailDto = _mapper.Map<ProductDetailDto>(existingProductDetail);
                return Result<ProductDetailDto>.Success(productDetailDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün detayı güncellenirken hata oluştu. ID: {Id}", id);
                return Result<ProductDetailDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün detayı güncellenemedi"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var productDetail = await _productDetailRepository.GetByIdAsync(id);
                if (productDetail == null)
                {
                    return Result<bool>.Failure(Error.NotFound("ProductDetail.NotFound", $"ID {id} olan ürün detayı bulunamadı"));
                }

                _productDetailRepository.Remove(productDetail);
                await _productDetailRepository.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün detayı silinirken hata oluştu. ID: {Id}", id);
                return Result<bool>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün detayı silinemedi"));
            }
        }

        public async Task<Result<IEnumerable<ProductDetailDto>>> GetActiveAsync()
        {
            try
            {
                var productDetails = await _productDetailRepository.FindAsync(pd => pd.IsActive);
                var productDetailDtos = _mapper.Map<IEnumerable<ProductDetailDto>>(productDetails);
                return Result<IEnumerable<ProductDetailDto>>.Success(productDetailDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aktif ürün detaylarını getirirken hata oluştu");
                return Result<IEnumerable<ProductDetailDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Aktif ürün detayları getirilemedi"));
            }
        }

        public async Task<Result<bool>> UpdateStatusAsync(int id, bool isActive)
        {
            try
            {
                var productDetail = await _productDetailRepository.GetByIdAsync(id);
                if (productDetail == null)
                {
                    return Result<bool>.Failure(Error.NotFound("ProductDetail.NotFound", $"ID {id} olan ürün detayı bulunamadı"));
                }

                productDetail.IsActive = isActive;
                productDetail.UpdatedAt = DateTime.UtcNow;

                _productDetailRepository.Update(productDetail);
                await _productDetailRepository.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün detayı durumu güncellenirken hata oluştu. ID: {Id}, Durum: {IsActive}", id, isActive);
                return Result<bool>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün detayı durumu güncellenemedi"));
            }
        }
    }
}
