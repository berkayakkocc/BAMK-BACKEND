using BAMK.Application.DTOs.TShirt;
using BAMK.Application.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TShirtController : ControllerBase
    {
        private readonly ITShirtService _tShirtService;
        private readonly ILogger<TShirtController> _logger;

        public TShirtController(ITShirtService tShirtService, ILogger<TShirtController> logger)
        {
            _tShirtService = tShirtService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm t-shirt'leri getirir
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TShirtDto>>> GetAll()
        {
            var result = await _tShirtService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Aktif t-shirt'leri getirir
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TShirtDto>>> GetActive()
        {
            var result = await _tShirtService.GetActiveAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// ID'ye göre t-shirt getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TShirtDto>> GetById(int id)
        {
            var result = await _tShirtService.GetByIdAsync(id);
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
        /// Renk bazlı t-shirt'leri getirir
        /// </summary>
        [HttpGet("color/{color}")]
        public async Task<ActionResult<IEnumerable<TShirtDto>>> GetByColor(string color)
        {
            var result = await _tShirtService.GetByColorAsync(color);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Beden bazlı t-shirt'leri getirir
        /// </summary>
        [HttpGet("size/{size}")]
        public async Task<ActionResult<IEnumerable<TShirtDto>>> GetBySize(string size)
        {
            var result = await _tShirtService.GetBySizeAsync(size);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Yeni t-shirt oluşturur (Admin yetkisi gerekli)
        /// </summary>
        [HttpPost]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult<TShirtDto>> Create([FromBody] CreateTShirtDto createTShirtDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tShirtService.CreateAsync(createTShirtDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// T-shirt günceller (Admin yetkisi gerekli)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult<TShirtDto>> Update(int id, [FromBody] UpdateTShirtDto updateTShirtDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tShirtService.UpdateAsync(id, updateTShirtDto);
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
        /// T-shirt stok miktarını günceller (Admin yetkisi gerekli)
        /// </summary>
        [HttpPut("{id}/stock")]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult> UpdateStock(int id, [FromBody] int quantity)
        {
            var result = await _tShirtService.UpdateStockAsync(id, quantity);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(new { message = "Stok başarıyla güncellendi" });
        }

        /// <summary>
        /// T-shirt siler (Admin yetkisi gerekli)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _tShirtService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(new { message = "T-shirt başarıyla silindi" });
        }
    }
}
