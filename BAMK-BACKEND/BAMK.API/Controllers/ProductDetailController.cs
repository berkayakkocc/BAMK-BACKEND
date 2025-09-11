using BAMK.Application.DTOs.ProductDetail;
using BAMK.Application.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;
        private readonly ILogger<ProductDetailController> _logger;

        public ProductDetailController(IProductDetailService productDetailService, ILogger<ProductDetailController> logger)
        {
            _productDetailService = productDetailService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm ürün detaylarını getirir
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetailDto>>> GetAll()
        {
            var result = await _productDetailService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Aktif ürün detaylarını getirir
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ProductDetailDto>>> GetActive()
        {
            var result = await _productDetailService.GetActiveAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// ID'ye göre ürün detayı getirir
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailDto>> GetById(int id)
        {
            var result = await _productDetailService.GetByIdAsync(id);
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
        /// TShirt ID'ye göre ürün detayı getirir
        /// </summary>
        [HttpGet("tshirt/{tShirtId}")]
        public async Task<ActionResult<ProductDetailDto>> GetByTShirtId(int tShirtId)
        {
            var result = await _productDetailService.GetByTShirtIdAsync(tShirtId);
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
        /// Yeni ürün detayı oluşturur (Admin yetkisi gerekli)
        /// </summary>
        [HttpPost]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult<ProductDetailDto>> Create([FromBody] CreateProductDetailDto createProductDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productDetailService.CreateAsync(createProductDetailDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Ürün detayını günceller (Admin yetkisi gerekli)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult<ProductDetailDto>> Update(int id, [FromBody] UpdateProductDetailDto updateProductDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productDetailService.UpdateAsync(id, updateProductDetailDto);
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
        /// Ürün detayı durumunu günceller (Admin yetkisi gerekli)
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] bool isActive)
        {
            var result = await _productDetailService.UpdateStatusAsync(id, isActive);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(new { message = "Ürün detayı durumu başarıyla güncellendi" });
        }

        /// <summary>
        /// Ürün detayını siler (Admin yetkisi gerekli)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize] // Geçici olarak tüm authenticated kullanıcılar
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _productDetailService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(new { message = "Ürün detayı başarıyla silindi" });
        }
    }
}
