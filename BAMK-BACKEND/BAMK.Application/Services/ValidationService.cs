using BAMK.Application.DTOs.TShirt;
using BAMK.Application.DTOs.Order;
using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using BAMK.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BAMK.Application.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IGenericRepository<TShirt> _tShirtRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(
            IGenericRepository<TShirt> tShirtRepository,
            IGenericRepository<User> userRepository,
            ILogger<ValidationService> logger)
        {
            _tShirtRepository = tShirtRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result> ValidateTShirtAsync(CreateTShirtDto dto)
        {
            try
            {
                var errors = new List<ValidationError>();

                // Name validation
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    errors.Add(new ValidationError("Name", "Ürün adı gereklidir"));
                }
                else if (dto.Name.Length > 200)
                {
                    errors.Add(new ValidationError("Name", "Ürün adı 200 karakterden fazla olamaz"));
                }

                // Price validation
                if (dto.Price <= 0)
                {
                    errors.Add(new ValidationError("Price", "Fiyat 0'dan büyük olmalıdır"));
                }
                else if (dto.Price > 10000)
                {
                    errors.Add(new ValidationError("Price", "Fiyat 10.000'den fazla olamaz"));
                }

                // Stock validation
                if (dto.StockQuantity < 0)
                {
                    errors.Add(new ValidationError("StockQuantity", "Stok miktarı negatif olamaz"));
                }
                else if (dto.StockQuantity > 10000)
                {
                    errors.Add(new ValidationError("StockQuantity", "Stok miktarı 10.000'den fazla olamaz"));
                }

                // Description validation
                if (!string.IsNullOrEmpty(dto.Description) && dto.Description.Length > 1000)
                {
                    errors.Add(new ValidationError("Description", "Açıklama 1000 karakterden fazla olamaz"));
                }

                // Image URL validation
                if (!string.IsNullOrEmpty(dto.ImageUrl) && dto.ImageUrl.Length > 500)
                {
                    errors.Add(new ValidationError("ImageUrl", "Resim URL'si 500 karakterden fazla olamaz"));
                }

                if (errors.Any())
                {
                    return Result.ValidationFailure(errors);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt validation sırasında hata oluştu");
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, "Validation hatası"));
            }
        }

        public async Task<Result> ValidateTShirtAsync(UpdateTShirtDto dto)
        {
            try
            {
                var errors = new List<ValidationError>();

                // Name validation
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    errors.Add(new ValidationError("Name", "Ürün adı gereklidir"));
                }
                else if (dto.Name.Length > 200)
                {
                    errors.Add(new ValidationError("Name", "Ürün adı 200 karakterden fazla olamaz"));
                }

                // Price validation
                if (dto.Price <= 0)
                {
                    errors.Add(new ValidationError("Price", "Fiyat 0'dan büyük olmalıdır"));
                }
                else if (dto.Price > 10000)
                {
                    errors.Add(new ValidationError("Price", "Fiyat 10.000'den fazla olamaz"));
                }

                // Stock validation
                if (dto.StockQuantity < 0)
                {
                    errors.Add(new ValidationError("StockQuantity", "Stok miktarı negatif olamaz"));
                }
                else if (dto.StockQuantity > 10000)
                {
                    errors.Add(new ValidationError("StockQuantity", "Stok miktarı 10.000'den fazla olamaz"));
                }

                // Description validation
                if (!string.IsNullOrEmpty(dto.Description) && dto.Description.Length > 1000)
                {
                    errors.Add(new ValidationError("Description", "Açıklama 1000 karakterden fazla olamaz"));
                }

                // Image URL validation
                if (!string.IsNullOrEmpty(dto.ImageUrl) && dto.ImageUrl.Length > 500)
                {
                    errors.Add(new ValidationError("ImageUrl", "Resim URL'si 500 karakterden fazla olamaz"));
                }

                if (errors.Any())
                {
                    return Result.ValidationFailure(errors);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "T-shirt update validation sırasında hata oluştu");
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, "Validation hatası"));
            }
        }

        public async Task<Result> ValidateOrderAsync(CreateOrderDto dto)
        {
            try
            {
                var errors = new List<ValidationError>();

                // User validation
                var userValidation = await ValidateUserAsync(dto.UserId);
                if (!userValidation.IsSuccess)
                {
                    errors.Add(new ValidationError("UserId", "Geçersiz kullanıcı"));
                }

                // Order items validation
                if (dto.OrderItems == null || !dto.OrderItems.Any())
                {
                    errors.Add(new ValidationError("OrderItems", "Sipariş en az bir ürün içermelidir"));
                }
                else
                {
                    foreach (var item in dto.OrderItems)
                    {
                        if (item.Quantity <= 0)
                        {
                            errors.Add(new ValidationError("OrderItems", "Ürün miktarı 0'dan büyük olmalıdır"));
                        }
                        else if (item.Quantity > 100)
                        {
                            errors.Add(new ValidationError("OrderItems", "Ürün miktarı 100'den fazla olamaz"));
                        }

                        // Stock validation for each item
                        var stockValidation = await ValidateStockAsync(item.TShirtId, item.Quantity);
                        if (!stockValidation.IsSuccess)
                        {
                            errors.Add(new ValidationError("OrderItems", $"Ürün ID {item.TShirtId} için yetersiz stok"));
                        }
                    }
                }

                // Shipping address validation
                if (string.IsNullOrWhiteSpace(dto.ShippingAddress))
                {
                    errors.Add(new ValidationError("ShippingAddress", "Teslimat adresi gereklidir"));
                }
                else if (dto.ShippingAddress.Length > 500)
                {
                    errors.Add(new ValidationError("ShippingAddress", "Teslimat adresi 500 karakterden fazla olamaz"));
                }

                if (errors.Any())
                {
                    return Result.ValidationFailure(errors);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş validation sırasında hata oluştu");
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, "Validation hatası"));
            }
        }

        public async Task<Result> ValidateStockAsync(int tShirtId, int requestedQuantity)
        {
            try
            {
                var tShirt = await _tShirtRepository.GetByIdAsync(tShirtId);
                if (tShirt == null)
                {
                    return Result.Failure(Error.NotFound($"T-shirt bulunamadı: {tShirtId}"));
                }

                if (!tShirt.IsActive)
                {
                    return Result.Failure(Error.Create(ErrorCode.ValidationError, "Ürün aktif değil"));
                }

                if (tShirt.StockQuantity < requestedQuantity)
                {
                    return Result.Failure(Error.Create(ErrorCode.ValidationError, $"Yetersiz stok. Mevcut: {tShirt.StockQuantity}, İstenen: {requestedQuantity}"));
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stok validation sırasında hata oluştu. TShirtId: {TShirtId}, Quantity: {Quantity}", tShirtId, requestedQuantity);
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, "Stok validation hatası"));
            }
        }

        public async Task<Result> ValidateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return Result.Failure(Error.NotFound($"Kullanıcı bulunamadı: {userId}"));
                }

                if (!user.IsActive)
                {
                    return Result.Failure(Error.Create(ErrorCode.ValidationError, "Kullanıcı aktif değil"));
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı validation sırasında hata oluştu. UserId: {UserId}", userId);
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, "Kullanıcı validation hatası"));
            }
        }
    }
}
