using AutoMapper;
using BAMK.Application.DTOs.Question;
using BAMK.Application.Services;
using BAMK.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BAMK.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionController : BaseController
{
    private readonly IQuestionService _questionService;
    private readonly IMapper _mapper;
    private readonly ILogger<QuestionController> _logger;

    public QuestionController(
        IQuestionService questionService,
        IMapper mapper,
        ILogger<QuestionController> logger)
    {
        _questionService = questionService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Tüm soruları getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _questionService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return ErrorResponse("Sorular getirilirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Sorular başarıyla getirildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sorular getirilirken hata oluştu");
            return ErrorResponse("Sorular getirilirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// ID'ye göre soru getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _questionService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Soru bulunamadı", 404);
                }
                return ErrorResponse("Soru getirilirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Soru başarıyla getirildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru getirilirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Soru getirilirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Kullanıcıya göre soruları getir
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        try
        {
            var result = await _questionService.GetByUserIdAsync(userId);
            if (!result.IsSuccess)
            {
                return ErrorResponse("Kullanıcı soruları getirilirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Kullanıcı soruları başarıyla getirildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı soruları getirilirken hata oluştu. UserId: {UserId}", userId);
            return ErrorResponse("Kullanıcı soruları getirilirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Yeni soru oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto createQuestionDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ErrorResponse("Geçersiz veri", 400, ModelState);
            }

            var result = await _questionService.CreateAsync(createQuestionDto);
            if (!result.IsSuccess)
            {
                return ErrorResponse("Soru oluşturulurken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Soru başarıyla oluşturuldu");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru oluşturulurken hata oluştu");
            return ErrorResponse("Soru oluşturulurken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Soru güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateQuestionDto updateQuestionDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ErrorResponse("Geçersiz veri", 400, ModelState);
            }

            var result = await _questionService.UpdateAsync(id, updateQuestionDto);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Soru bulunamadı", 404);
                }
                return ErrorResponse("Soru güncellenirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Soru başarıyla güncellendi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru güncellenirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Soru güncellenirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Soru sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _questionService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Soru bulunamadı", 404);
                }
                return ErrorResponse("Soru silinirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse("Soru başarıyla silindi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru silinirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Soru silinirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Soruyu aktifleştir
    /// </summary>
    [HttpPut("{id}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        try
        {
            var result = await _questionService.ActivateAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Soru bulunamadı", 404);
                }
                return ErrorResponse("Soru aktifleştirilirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse("Soru başarıyla aktifleştirildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru aktifleştirilirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Soru aktifleştirilirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Soruyu deaktifleştir
    /// </summary>
    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        try
        {
            var result = await _questionService.DeactivateAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Soru bulunamadı", 404);
                }
                return ErrorResponse("Soru deaktifleştirilirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse("Soru başarıyla deaktifleştirildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru deaktifleştirilirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Soru deaktifleştirilirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Soruya cevap ekle
    /// </summary>
    [HttpPost("{questionId}/answers")]
    public async Task<IActionResult> CreateAnswer(int questionId, [FromBody] CreateAnswerDto createAnswerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ErrorResponse("Geçersiz veri", 400, ModelState);
            }

            createAnswerDto.QuestionId = questionId;
            var result = await _questionService.CreateAnswerAsync(createAnswerDto);
            if (!result.IsSuccess)
            {
                return ErrorResponse("Cevap oluşturulurken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Cevap başarıyla oluşturuldu");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap oluşturulurken hata oluştu. QuestionId: {QuestionId}", questionId);
            return ErrorResponse("Cevap oluşturulurken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Soru cevaplarını getir
    /// </summary>
    [HttpGet("{questionId}/answers")]
    public async Task<IActionResult> GetAnswers(int questionId)
    {
        try
        {
            var result = await _questionService.GetAnswersByQuestionIdAsync(questionId);
            if (!result.IsSuccess)
            {
                return ErrorResponse("Soru cevapları getirilirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Soru cevapları başarıyla getirildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru cevapları getirilirken hata oluştu. QuestionId: {QuestionId}", questionId);
            return ErrorResponse("Soru cevapları getirilirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Cevap getir
    /// </summary>
    [HttpGet("answers/{id}")]
    public async Task<IActionResult> GetAnswerById(int id)
    {
        try
        {
            var result = await _questionService.GetAnswerByIdAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Cevap bulunamadı", 404);
                }
                return ErrorResponse("Cevap getirilirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Cevap başarıyla getirildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap getirilirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Cevap getirilirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Cevap güncelle
    /// </summary>
    [HttpPut("answers/{id}")]
    public async Task<IActionResult> UpdateAnswer(int id, [FromBody] UpdateAnswerDto updateAnswerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ErrorResponse("Geçersiz veri", 400, ModelState);
            }

            var result = await _questionService.UpdateAnswerAsync(id, updateAnswerDto);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Cevap bulunamadı", 404);
                }
                return ErrorResponse("Cevap güncellenirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse(result.Value, "Cevap başarıyla güncellendi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap güncellenirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Cevap güncellenirken hata oluştu", 500);
        }
    }

    /// <summary>
    /// Cevap sil
    /// </summary>
    [HttpDelete("answers/{id}")]
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        try
        {
            var result = await _questionService.DeleteAnswerAsync(id);
            if (!result.IsSuccess)
            {
                if (result.Error?.Code == ErrorCode.NotFound)
                {
                    return ErrorResponse("Cevap bulunamadı", 404);
                }
                return ErrorResponse("Cevap silinirken hata oluştu", 400, result.Error);
            }

            return SuccessResponse("Cevap başarıyla silindi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap silinirken hata oluştu. ID: {Id}", id);
            return ErrorResponse("Cevap silinirken hata oluştu", 500);
        }
    }
}
