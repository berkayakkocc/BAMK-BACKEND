using AutoMapper;
using BAMK.Application.DTOs.Cart;
using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using BAMK.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BAMK.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        private readonly IGenericRepository<TShirt> _tShirtRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(
            IGenericRepository<Cart> cartRepository,
            IGenericRepository<CartItem> cartItemRepository,
            IGenericRepository<TShirt> tShirtRepository,
            IGenericRepository<User> userRepository,
            IMapper mapper,
            ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _tShirtRepository = tShirtRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CartDto>> GetCartAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Cart getiriliyor. UserId: {UserId}", userId);
                
                var cart = await _cartRepository.FindWithIncludesAsync(
                    c => c.UserId == userId,
                    c => c.CartItems
                );

                var userCart = cart.FirstOrDefault();
                if (userCart == null)
                {
                    _logger.LogInformation("Cart bulunamadı, yeni cart oluşturuluyor. UserId: {UserId}", userId);
                    
                    // Create empty cart for user
                    userCart = new Cart
                    {
                        UserId = userId,
                        TotalAmount = 0,
                        TotalItems = 0,
                        LastUpdated = DateTime.UtcNow
                    };
                    
                    await _cartRepository.AddAsync(userCart);
                    await _cartRepository.SaveChangesAsync(); // Save changes
                    
                    _logger.LogInformation("Yeni cart oluşturuldu. UserId: {UserId}, CartId: {CartId}", userId, userCart.Id);
                }
                else
                {
                    _logger.LogInformation("Mevcut cart bulundu. UserId: {UserId}, CartId: {CartId}", userId, userCart.Id);
                    
                    // Load TShirt details for each cart item
                    foreach (var cartItem in userCart.CartItems)
                    {
                        if (cartItem.TShirt == null)
                        {
                            var tShirt = await _tShirtRepository.GetByIdAsync(cartItem.TShirtId);
                            if (tShirt != null)
                            {
                                cartItem.TShirt = tShirt;
                            }
                        }
                    }
                }

                var cartDto = _mapper.Map<CartDto>(userCart);
                return Result<CartDto>.Success(cartDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet getirilirken hata oluştu. UserId: {UserId}", userId);
                return Result<CartDto>.Failure(Error.Create(ErrorCode.InvalidOperation, $"Sepet getirilemedi: {ex.Message}"));
            }
        }

        public async Task<Result<CartItemDto>> AddToCartAsync(int userId, AddToCartDto dto)
        {
            try
            {
                // Validate user
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return Result<CartItemDto>.Failure(Error.NotFound("Kullanıcı bulunamadı"));
                }

                // Validate product
                var tShirt = await _tShirtRepository.GetByIdAsync(dto.TShirtId);
                if (tShirt == null || !tShirt.IsActive)
                {
                    return Result<CartItemDto>.Failure(Error.NotFound("Ürün bulunamadı veya aktif değil"));
                }

                // Check stock
                if (tShirt.StockQuantity < dto.Quantity)
                {
                    return Result<CartItemDto>.Failure(Error.Create(ErrorCode.ValidationError, "Yeterli stok bulunmuyor"));
                }

                // Get or create cart
                var cart = await GetOrCreateCartAsync(userId);

                // Check if item already exists in cart
                var existingItem = cart.CartItems.FirstOrDefault(ci => ci.TShirtId == dto.TShirtId);
                if (existingItem != null)
                {
                    // Update quantity
                    existingItem.Quantity += dto.Quantity;
                    existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
                    _cartItemRepository.Update(existingItem);
                }
                else
                {
                    // Add new item
                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        TShirtId = dto.TShirtId,
                        Quantity = dto.Quantity,
                        UnitPrice = tShirt.Price,
                        TotalPrice = dto.Quantity * tShirt.Price,
                        AddedAt = DateTime.UtcNow
                    };
                    await _cartItemRepository.AddAsync(cartItem);
                    cart.CartItems.Add(cartItem);
                }

                // Update cart totals
                await UpdateCartTotalsAsync(cart);
                
                // Save changes
                await _cartRepository.SaveChangesAsync();

                var cartItemDto = _mapper.Map<CartItemDto>(existingItem ?? cart.CartItems.Last());
                return Result<CartItemDto>.Success(cartItemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün sepete eklenirken hata oluştu. UserId: {UserId}, TShirtId: {TShirtId}", userId, dto.TShirtId);
                return Result<CartItemDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ürün sepete eklenemedi"));
            }
        }

        public async Task<Result<CartItemDto>> UpdateCartItemAsync(int userId, UpdateCartItemDto dto)
        {
            try
            {
                var cartItem = await _cartItemRepository.FindWithIncludesAsync(
                    ci => ci.Id == dto.CartItemId && ci.Cart.UserId == userId,
                    ci => ci.Cart,
                    ci => ci.TShirt
                );

                var item = cartItem.FirstOrDefault();
                if (item == null)
                {
                    return Result<CartItemDto>.Failure(Error.NotFound("Sepet öğesi bulunamadı"));
                }

                // Validate quantity
                if (dto.Quantity <= 0)
                {
                    return Result<CartItemDto>.Failure(Error.Create(ErrorCode.ValidationError, "Miktar 0'dan büyük olmalıdır"));
                }

                if (item.TShirt.StockQuantity < dto.Quantity)
                {
                    return Result<CartItemDto>.Failure(Error.Create(ErrorCode.ValidationError, "Yeterli stok bulunmuyor"));
                }

                // Update quantity
                item.Quantity = dto.Quantity;
                item.TotalPrice = item.Quantity * item.UnitPrice;
                _cartItemRepository.Update(item);

                // Update cart totals
                await UpdateCartTotalsAsync(item.Cart);

                var cartItemDto = _mapper.Map<CartItemDto>(item);
                return Result<CartItemDto>.Success(cartItemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet öğesi güncellenirken hata oluştu. UserId: {UserId}, CartItemId: {CartItemId}", userId, dto.CartItemId);
                return Result<CartItemDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Sepet öğesi güncellenemedi"));
            }
        }

        public async Task<Result> RemoveFromCartAsync(int userId, int cartItemId)
        {
            try
            {
                var cartItem = await _cartItemRepository.FindWithIncludesAsync(
                    ci => ci.Id == cartItemId && ci.Cart.UserId == userId,
                    ci => ci.Cart
                );

                var item = cartItem.FirstOrDefault();
                if (item == null)
                {
                    return Result.Failure(Error.NotFound("Sepet öğesi bulunamadı"));
                }

                _cartItemRepository.Remove(item);

                // Update cart totals
                await UpdateCartTotalsAsync(item.Cart);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet öğesi silinirken hata oluştu. UserId: {UserId}, CartItemId: {CartItemId}", userId, cartItemId);
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, "Sepet öğesi silinemedi"));
            }
        }

        public async Task<Result> ClearCartAsync(int userId)
        {
            try
            {
                var cart = await _cartRepository.FindAsync(c => c.UserId == userId);
                var userCart = cart.FirstOrDefault();
                if (userCart != null)
                {
                    // Remove all cart items
                    var cartItems = await _cartItemRepository.FindAsync(ci => ci.CartId == userCart.Id);
                    foreach (var item in cartItems)
                    {
                        _cartItemRepository.Remove(item);
                    }

                    // Reset cart totals
                    userCart.TotalAmount = 0;
                    userCart.TotalItems = 0;
                    userCart.LastUpdated = DateTime.UtcNow;
                    _cartRepository.Update(userCart);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet temizlenirken hata oluştu. UserId: {UserId}", userId);
                return Result.Failure(Error.Create(ErrorCode.InvalidOperation, "Sepet temizlenemedi"));
            }
        }

        public async Task<Result<CartSummaryDto>> GetCartSummaryAsync(int userId)
        {
            try
            {
                var cart = await _cartRepository.FindWithIncludesAsync(
                    c => c.UserId == userId,
                    c => c.CartItems,
                    c => c.CartItems.Select(ci => ci.TShirt)
                );

                var userCart = cart.FirstOrDefault();
                if (userCart == null)
                {
                    return Result<CartSummaryDto>.Success(new CartSummaryDto
                    {
                        TotalItems = 0,
                        TotalAmount = 0,
                        Items = new List<CartItemDto>()
                    });
                }

                var summary = new CartSummaryDto
                {
                    TotalItems = userCart.TotalItems,
                    TotalAmount = userCart.TotalAmount,
                    Items = _mapper.Map<List<CartItemDto>>(userCart.CartItems)
                };

                return Result<CartSummaryDto>.Success(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sepet özeti getirilirken hata oluştu. UserId: {UserId}", userId);
                return Result<CartSummaryDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Sepet özeti getirilemedi"));
            }
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _cartRepository.FindAsync(c => c.UserId == userId);
            var userCart = cart.FirstOrDefault();
            
            if (userCart == null)
            {
                userCart = new Cart
                {
                    UserId = userId,
                    TotalAmount = 0,
                    TotalItems = 0,
                    LastUpdated = DateTime.UtcNow
                };
                await _cartRepository.AddAsync(userCart);
                await _cartRepository.SaveChangesAsync(); // Save changes
            }

            return userCart;
        }

        private async Task UpdateCartTotalsAsync(Cart cart)
        {
            var cartItems = await _cartItemRepository.FindAsync(ci => ci.CartId == cart.Id);
            
            cart.TotalItems = cartItems.Sum(ci => ci.Quantity);
            cart.TotalAmount = cartItems.Sum(ci => ci.TotalPrice);
            cart.LastUpdated = DateTime.UtcNow;
            
            _cartRepository.Update(cart);
        }
    }
}
