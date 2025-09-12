using BAMK.Application.DTOs.TShirt;
using BAMK.Application.DTOs.Order;
using BAMK.Core.Common;

namespace BAMK.Application.Services
{
    public interface IValidationService
    {
        Task<Result> ValidateTShirtAsync(CreateTShirtDto dto);
        Task<Result> ValidateTShirtAsync(UpdateTShirtDto dto);
        Task<Result> ValidateOrderAsync(CreateOrderDto dto);
        Task<Result> ValidateStockAsync(int tShirtId, int requestedQuantity);
        Task<Result> ValidateUserAsync(int userId);
    }
}
