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
    public class OrderController : ControllerBase
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
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var result = await _orderService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// ID'ye göre sipariş getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var result = await _orderService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Kullanıcının siparişlerini getirir
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByUserId(int userId)
        {
            var result = await _orderService.GetByUserIdAsync(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Durum bazlı siparişleri getirir
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByStatus(string status)
        {
            var result = await _orderService.GetByStatusAsync(status);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Yeni sipariş oluşturur
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.CreateAsync(createOrderDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Sipariş durumunu günceller (Admin)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<ActionResult<OrderDto>> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto updateStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.UpdateStatusAsync(id, updateStatusDto);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Ödeme durumunu günceller (Admin)
        /// </summary>
        [HttpPut("{id}/payment-status")]
        public async Task<ActionResult<OrderDto>> UpdatePaymentStatus(int id, [FromBody] UpdatePaymentStatusDto updatePaymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.UpdatePaymentStatusAsync(id, updatePaymentDto);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Sipariş siler (Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(new { message = "Sipariş başarıyla silindi" });
        }

        /// <summary>
        /// Sipariş toplam tutarını hesaplar
        /// </summary>
        [HttpPost("calculate-total")]
        public async Task<ActionResult<decimal>> CalculateTotal([FromBody] List<CreateOrderItemDto> orderItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.CalculateTotalAsync(orderItems);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { total = result.Value });
        }
    }
}
