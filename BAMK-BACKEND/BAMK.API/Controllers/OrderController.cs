using BAMK.Application.DTOs.Order;
using BAMK.Application.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Tüm sipariş işlemleri için authentication gerekli
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm siparişleri getirir (Admin)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _orderService.GetAllAsync();
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Siparişler getirilirken hata oluştu", 400, result.Error);
                }
                return SuccessResponse(result.Value, "Siparişler başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Siparişler getirilirken hata oluştu");
                return ErrorResponse("Siparişler getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// ID'ye göre sipariş getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _orderService.GetByIdAsync(id);
                if (!result.IsSuccess)
                {
                    if (result.Error?.Code == ErrorCode.NotFound)
                    {
                        return ErrorResponse("Sipariş bulunamadı", 404);
                    }
                    return ErrorResponse("Sipariş getirilirken hata oluştu", 400, result.Error);
                }
                return SuccessResponse(result.Value, "Sipariş başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş getirilirken hata oluştu. ID: {Id}", id);
                return ErrorResponse("Sipariş getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Kullanıcının siparişlerini getirir
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var result = await _orderService.GetByUserIdAsync(userId);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Kullanıcı siparişleri getirilirken hata oluştu", 400, result.Error);
                }
                return SuccessResponse(result.Value, "Kullanıcı siparişleri başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı siparişleri getirilirken hata oluştu. UserId: {UserId}", userId);
                return ErrorResponse("Kullanıcı siparişleri getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Durum bazlı siparişleri getirir
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            try
            {
                var result = await _orderService.GetByStatusAsync(status);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Durum bazlı siparişler getirilirken hata oluştu", 400, result.Error);
                }
                return SuccessResponse(result.Value, "Durum bazlı siparişler başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Durum bazlı siparişler getirilirken hata oluştu. Durum: {Status}", status);
                return ErrorResponse("Durum bazlı siparişler getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yeni sipariş oluşturur
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResponse("Geçersiz veri", 400, ModelState);
                }

                var result = await _orderService.CreateAsync(createOrderDto);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Sipariş oluşturulurken hata oluştu", 400, result.Error);
                }

                return SuccessResponse(result.Value, "Sipariş başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
                return ErrorResponse("Sipariş oluşturulurken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sipariş durumunu günceller (Admin)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto updateStatusDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResponse("Geçersiz veri", 400, ModelState);
                }

                var result = await _orderService.UpdateStatusAsync(id, updateStatusDto);
                if (!result.IsSuccess)
                {
                    if (result.Error?.Code == ErrorCode.NotFound)
                    {
                        return ErrorResponse("Sipariş bulunamadı", 404);
                    }
                    return ErrorResponse("Sipariş durumu güncellenirken hata oluştu", 400, result.Error);
                }

                return SuccessResponse(result.Value, "Sipariş durumu başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş durumu güncellenirken hata oluştu. ID: {Id}", id);
                return ErrorResponse("Sipariş durumu güncellenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Ödeme durumunu günceller (Admin)
        /// </summary>
        [HttpPut("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] UpdatePaymentStatusDto updatePaymentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResponse("Geçersiz veri", 400, ModelState);
                }

                var result = await _orderService.UpdatePaymentStatusAsync(id, updatePaymentDto);
                if (!result.IsSuccess)
                {
                    if (result.Error?.Code == ErrorCode.NotFound)
                    {
                        return ErrorResponse("Sipariş bulunamadı", 404);
                    }
                    return ErrorResponse("Ödeme durumu güncellenirken hata oluştu", 400, result.Error);
                }

                return SuccessResponse(result.Value, "Ödeme durumu başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ödeme durumu güncellenirken hata oluştu. ID: {Id}", id);
                return ErrorResponse("Ödeme durumu güncellenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sipariş siler (Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _orderService.DeleteAsync(id);
                if (!result.IsSuccess)
                {
                    if (result.Error?.Code == ErrorCode.NotFound)
                    {
                        return ErrorResponse("Sipariş bulunamadı", 404);
                    }
                    return ErrorResponse("Sipariş silinirken hata oluştu", 400, result.Error);
                }

                return SuccessResponse("Sipariş başarıyla silindi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş silinirken hata oluştu. ID: {Id}", id);
                return ErrorResponse("Sipariş silinirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Sipariş toplam tutarını hesaplar
        /// </summary>
        [HttpPost("calculate-total")]
        public async Task<IActionResult> CalculateTotal([FromBody] List<CreateOrderItemDto> orderItems)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ErrorResponse("Geçersiz veri", 400, ModelState);
                }

                var result = await _orderService.CalculateTotalAsync(orderItems);
                if (!result.IsSuccess)
                {
                    return ErrorResponse("Toplam tutar hesaplanırken hata oluştu", 400, result.Error);
                }

                return SuccessResponse(new { total = result.Value }, "Toplam tutar başarıyla hesaplandı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Toplam tutar hesaplanırken hata oluştu");
                return ErrorResponse("Toplam tutar hesaplanırken hata oluştu", 500);
            }
        }
    }
}
