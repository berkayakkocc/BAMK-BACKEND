using BAMK.Core.Common;

namespace BAMK.API.Services
{
    public interface ICartService
    {
        Task<Result<object>> AddToCartAsync(string productId, int quantity);
        Task<Result<object>> GetCartAsync();
        Task<Result<object>> GetCartSummaryAsync();
        Task<Result> ClearCartAsync();
    }
}
