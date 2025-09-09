using BAMK.Application.DTOs.Order;
using BAMK.Core.Common;

namespace BAMK.Application.Services
{
    public interface IOrderService
    {
        Task<Result<IEnumerable<OrderDto>>> GetAllAsync();
        Task<Result<OrderDto?>> GetByIdAsync(int id);
        Task<Result<IEnumerable<OrderDto>>> GetByUserIdAsync(int userId);
        Task<Result<OrderDto>> CreateAsync(CreateOrderDto createOrderDto);
        Task<Result<OrderDto>> UpdateStatusAsync(int id, UpdateOrderStatusDto updateStatusDto);
        Task<Result<OrderDto>> UpdatePaymentStatusAsync(int id, UpdatePaymentStatusDto updatePaymentDto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<IEnumerable<OrderDto>>> GetByStatusAsync(string status);
        Task<Result<decimal>> CalculateTotalAsync(List<CreateOrderItemDto> orderItems);
    }
}
