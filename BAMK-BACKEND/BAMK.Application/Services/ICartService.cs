using BAMK.Application.DTOs.Cart;
using BAMK.Core.Common;

namespace BAMK.Application.Services
{
    public interface ICartService
    {
        Task<Result<CartDto>> GetCartAsync(int userId);
        Task<Result<CartItemDto>> AddToCartAsync(int userId, AddToCartDto dto);
        Task<Result<CartItemDto>> UpdateCartItemAsync(int userId, UpdateCartItemDto dto);
        Task<Result> RemoveFromCartAsync(int userId, int cartItemId);
        Task<Result> ClearCartAsync(int userId);
        Task<Result<CartSummaryDto>> GetCartSummaryAsync(int userId);
    }
}
