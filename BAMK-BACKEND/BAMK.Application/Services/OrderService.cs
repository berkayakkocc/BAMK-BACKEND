using AutoMapper;
using BAMK.Application.DTOs.Order;
using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using BAMK.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BAMK.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly IGenericRepository<TShirt> _tShirtRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IGenericRepository<OrderItem> orderItemRepository,
            IGenericRepository<TShirt> tShirtRepository,
            IGenericRepository<User> userRepository,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _tShirtRepository = tShirtRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<OrderDto>>> GetAllAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
                return Result<IEnumerable<OrderDto>>.Success(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm siparişleri getirirken hata oluştu");
                return Result<IEnumerable<OrderDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Siparişler getirilemedi"));
            }
        }

        public async Task<Result<OrderDto?>> GetByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdWithIncludesAsync(id, 
                    o => o.User, 
                    o => o.OrderItems, 
                    o => o.OrderItems.Select(oi => oi.TShirt));
                    
                if (order == null)
                {
                    return Result<OrderDto?>.Failure(Error.NotFound($"ID {id} olan sipariş bulunamadı"));
                }

                var orderDto = _mapper.Map<OrderDto>(order);
                return Result<OrderDto?>.Success(orderDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş getirirken hata oluştu. ID: {Id}", id);
                return Result<OrderDto?>.Failure(Error.Create(ErrorCode.InvalidOperation, "Sipariş getirilemedi"));
            }
        }

        public async Task<Result<IEnumerable<OrderDto>>> GetByUserIdAsync(int userId)
        {
            try
            {
                var orders = await _orderRepository.FindWithIncludesAsync(o => o.UserId == userId,
                    o => o.User,
                    o => o.OrderItems,
                    o => o.OrderItems.Select(oi => oi.TShirt));
                    
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
                return Result<IEnumerable<OrderDto>>.Success(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı siparişlerini getirirken hata oluştu. UserId: {UserId}", userId);
                return Result<IEnumerable<OrderDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Kullanıcı siparişleri getirilemedi"));
            }
        }

        public async Task<Result<OrderDto>> CreateAsync(CreateOrderDto createOrderDto)
        {
            try
            {
                // Transaction başlat
                using var transaction = await _orderRepository.BeginTransactionAsync();
                
                try
                {
                    // Kullanıcı kontrolü
                    var user = await _userRepository.GetByIdAsync(createOrderDto.UserId);
                    if (user == null)
                    {
                        return Result<OrderDto>.Failure(Error.NotFound($"Kullanıcı bulunamadı: {createOrderDto.UserId}"));
                    }

                    // Toplam tutarı hesapla
                    var totalResult = await CalculateTotalAsync(createOrderDto.OrderItems);
                    if (!totalResult.IsSuccess)
                    {
                        return Result<OrderDto>.Failure(totalResult.Error!);
                    }

                    // Sipariş oluştur
                    var order = new Order
                    {
                        UserId = createOrderDto.UserId,
                        TotalAmount = totalResult.Value,
                        OrderStatus = "Pending",
                        PaymentStatus = "Pending",
                        ShippingAddress = createOrderDto.ShippingAddress,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _orderRepository.AddAsync(order);
                    await _orderRepository.SaveChangesAsync();

                    // Sipariş kalemlerini oluştur
                    foreach (var itemDto in createOrderDto.OrderItems)
                    {
                        var tShirt = await _tShirtRepository.GetByIdAsync(itemDto.TShirtId);
                        if (tShirt == null)
                        {
                            await transaction.RollbackAsync();
                            return Result<OrderDto>.Failure(Error.NotFound($"T-shirt bulunamadı: {itemDto.TShirtId}"));
                        }

                        // Stok kontrolü ve güncelleme
                        if (tShirt.StockQuantity < itemDto.Quantity)
                        {
                            await transaction.RollbackAsync();
                            return Result<OrderDto>.Failure(Error.Create(ErrorCode.ValidationError, $"Yetersiz stok: {tShirt.Name}"));
                        }

                        tShirt.StockQuantity -= itemDto.Quantity;
                        _tShirtRepository.Update(tShirt);

                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            TShirtId = itemDto.TShirtId,
                            UnitPrice = tShirt.Price,
                            Quantity = itemDto.Quantity,
                            TotalPrice = tShirt.Price * itemDto.Quantity
                        };

                        await _orderItemRepository.AddAsync(orderItem);
                    }

                    await _orderItemRepository.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var orderDto = _mapper.Map<OrderDto>(order);
                    return Result<OrderDto>.Success(orderDto);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluştururken hata oluştu");
                return Result<OrderDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Sipariş oluşturulamadı"));
            }
        }

        public async Task<Result<OrderDto>> UpdateStatusAsync(int id, UpdateOrderStatusDto updateStatusDto)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return Result<OrderDto>.Failure(Error.NotFound($"ID {id} olan sipariş bulunamadı"));
                }

                order.OrderStatus = updateStatusDto.Status;
                order.UpdatedAt = DateTime.UtcNow;

                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();

                var orderDto = _mapper.Map<OrderDto>(order);
                return Result<OrderDto>.Success(orderDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş durumu güncellenirken hata oluştu. ID: {Id}", id);
                return Result<OrderDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Sipariş durumu güncellenemedi"));
            }
        }

        public async Task<Result<OrderDto>> UpdatePaymentStatusAsync(int id, UpdatePaymentStatusDto updatePaymentDto)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return Result<OrderDto>.Failure(Error.NotFound($"ID {id} olan sipariş bulunamadı"));
                }

                order.PaymentStatus = updatePaymentDto.PaymentStatus;
                order.UpdatedAt = DateTime.UtcNow;

                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();

                var orderDto = _mapper.Map<OrderDto>(order);
                return Result<OrderDto>.Success(orderDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ödeme durumu güncellenirken hata oluştu. ID: {Id}", id);
                return Result<OrderDto>.Failure(Error.Create(ErrorCode.InvalidOperation, "Ödeme durumu güncellenemedi"));
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return Result<bool>.Failure(Error.NotFound($"ID {id} olan sipariş bulunamadı"));
                }

                _orderRepository.Remove(order);
                await _orderRepository.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş silinirken hata oluştu. ID: {Id}", id);
                return Result<bool>.Failure(Error.Create(ErrorCode.InvalidOperation, "Sipariş silinemedi"));
            }
        }

        public async Task<Result<IEnumerable<OrderDto>>> GetByStatusAsync(string status)
        {
            try
            {
                var orders = await _orderRepository.FindAsync(o => o.OrderStatus == status);
                var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
                return Result<IEnumerable<OrderDto>>.Success(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Durum bazlı siparişleri getirirken hata oluştu. Durum: {Status}", status);
                return Result<IEnumerable<OrderDto>>.Failure(Error.Create(ErrorCode.InvalidOperation, "Durum bazlı siparişler getirilemedi"));
            }
        }

        public async Task<Result<decimal>> CalculateTotalAsync(List<CreateOrderItemDto> orderItems)
        {
            try
            {
                decimal total = 0;

                foreach (var item in orderItems)
                {
                    var tShirt = await _tShirtRepository.GetByIdAsync(item.TShirtId);
                    if (tShirt == null)
                    {
                        return Result<decimal>.Failure(Error.NotFound($"T-shirt bulunamadı: {item.TShirtId}"));
                    }

                    if (!tShirt.IsActive)
                    {
                        return Result<decimal>.Failure(Error.Create(ErrorCode.InvalidOperation, $"T-shirt aktif değil: {tShirt.Name}"));
                    }

                    if (tShirt.StockQuantity < item.Quantity)
                    {
                        return Result<decimal>.Failure(Error.Create(ErrorCode.InvalidOperation, $"Yetersiz stok: {tShirt.Name}"));
                    }

                    total += tShirt.Price * item.Quantity;
                }

                return Result<decimal>.Success(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Toplam tutar hesaplanırken hata oluştu");
                return Result<decimal>.Failure(Error.Create(ErrorCode.InvalidOperation, "Toplam tutar hesaplanamadı"));
            }
        }
    }
}
