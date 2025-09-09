using AutoMapper;
using BAMK.Application.DTOs.Question;
using BAMK.Core.Common;
using BAMK.Core.Interfaces;
using BAMK.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BAMK.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<Answer> _answerRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<QuestionService> _logger;

    public QuestionService(
        IGenericRepository<Question> questionRepository,
        IGenericRepository<Answer> answerRepository,
        IGenericRepository<User> userRepository,
        IMapper mapper,
        ILogger<QuestionService> logger)
    {
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<QuestionDto>> GetByIdAsync(int id)
    {
        try
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return Result<QuestionDto>.Failure(Error.Create(ErrorCode.NotFound, "Soru bulunamadı"));
            }

            var questionDto = _mapper.Map<QuestionDto>(question);
            return Result<QuestionDto>.Success(questionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru getirilirken hata oluştu: {QuestionId}", id);
            return Result<QuestionDto>.Failure(Error.Create(ErrorCode.InternalServerError, "Soru getirilirken hata oluştu"));
        }
    }

    public async Task<Result<IEnumerable<QuestionDto>>> GetAllAsync()
    {
        try
        {
            var questions = await _questionRepository.GetAllAsync();
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(questions);
            return Result<IEnumerable<QuestionDto>>.Success(questionDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sorular getirilirken hata oluştu");
            return Result<IEnumerable<QuestionDto>>.Failure(Error.Create(ErrorCode.InternalServerError, "Sorular getirilirken hata oluştu"));
        }
    }

    public async Task<Result<IEnumerable<QuestionDto>>> GetByUserIdAsync(int userId)
    {
        try
        {
            var questions = await _questionRepository.GetAllAsync();
            var userQuestions = questions.Where(q => q.UserId == userId);
            var questionDtos = _mapper.Map<IEnumerable<QuestionDto>>(userQuestions);
            return Result<IEnumerable<QuestionDto>>.Success(questionDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı soruları getirilirken hata oluştu: {UserId}", userId);
            return Result<IEnumerable<QuestionDto>>.Failure(Error.Create(ErrorCode.InternalServerError, "Kullanıcı soruları getirilirken hata oluştu"));
        }
    }

    public async Task<Result<QuestionDto>> CreateAsync(CreateQuestionDto createQuestionDto)
    {
        try
        {
            // Kullanıcı kontrolü
            var user = await _userRepository.GetByIdAsync(createQuestionDto.UserId);
            if (user == null)
            {
                return Result<QuestionDto>.Failure(Error.Create(ErrorCode.NotFound, "Kullanıcı bulunamadı"));
            }

            var question = _mapper.Map<Question>(createQuestionDto);
            question.CreatedAt = DateTime.UtcNow;
            question.IsActive = true;

            await _questionRepository.AddAsync(question);
            await _questionRepository.SaveChangesAsync();

            var questionDto = _mapper.Map<QuestionDto>(question);
            return Result<QuestionDto>.Success(questionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru oluşturulurken hata oluştu");
            return Result<QuestionDto>.Failure(Error.Create(ErrorCode.InternalServerError, "Soru oluşturulurken hata oluştu"));
        }
    }

    public async Task<Result<QuestionDto>> UpdateAsync(int id, UpdateQuestionDto updateQuestionDto)
    {
        try
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return Result<QuestionDto>.Failure(Error.Create(ErrorCode.NotFound, "Soru bulunamadı"));
            }

            question.Title = updateQuestionDto.Title;
            question.Content = updateQuestionDto.Content;
            question.UpdatedAt = DateTime.UtcNow;

            await _questionRepository.UpdateAsync(question);
            await _questionRepository.SaveChangesAsync();

            var questionDto = _mapper.Map<QuestionDto>(question);
            return Result<QuestionDto>.Success(questionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru güncellenirken hata oluştu: {QuestionId}", id);
            return Result<QuestionDto>.Failure(Error.Create(ErrorCode.InternalServerError, "Soru güncellenirken hata oluştu"));
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return Result<bool>.Failure(Error.Create(ErrorCode.NotFound, "Soru bulunamadı"));
            }

            await _questionRepository.DeleteAsync(question);
            await _questionRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru silinirken hata oluştu: {QuestionId}", id);
            return Result<bool>.Failure(Error.Create(ErrorCode.InternalServerError, "Soru silinirken hata oluştu"));
        }
    }

    public async Task<Result<bool>> ActivateAsync(int id)
    {
        try
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return Result<bool>.Failure(Error.Create(ErrorCode.NotFound, "Soru bulunamadı"));
            }

            question.IsActive = true;
            question.UpdatedAt = DateTime.UtcNow;

            await _questionRepository.UpdateAsync(question);
            await _questionRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru aktifleştirilirken hata oluştu: {QuestionId}", id);
            return Result<bool>.Failure(Error.Create(ErrorCode.InternalServerError, "Soru aktifleştirilirken hata oluştu"));
        }
    }

    public async Task<Result<bool>> DeactivateAsync(int id)
    {
        try
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return Result<bool>.Failure(Error.Create(ErrorCode.NotFound, "Soru bulunamadı"));
            }

            question.IsActive = false;
            question.UpdatedAt = DateTime.UtcNow;

            await _questionRepository.UpdateAsync(question);
            await _questionRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru deaktifleştirilirken hata oluştu: {QuestionId}", id);
            return Result<bool>.Failure(Error.Create(ErrorCode.InternalServerError, "Soru deaktifleştirilirken hata oluştu"));
        }
    }

    public async Task<Result<AnswerDto>> GetAnswerByIdAsync(int id)
    {
        try
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Result<AnswerDto>.Failure(Error.Create(ErrorCode.NotFound, "Cevap bulunamadı"));
            }

            var answerDto = _mapper.Map<AnswerDto>(answer);
            return Result<AnswerDto>.Success(answerDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap getirilirken hata oluştu: {AnswerId}", id);
            return Result<AnswerDto>.Failure(Error.Create(ErrorCode.InternalServerError, "Cevap getirilirken hata oluştu"));
        }
    }

    public async Task<Result<IEnumerable<AnswerDto>>> GetAnswersByQuestionIdAsync(int questionId)
    {
        try
        {
            var answers = await _answerRepository.GetAllAsync();
            var questionAnswers = answers.Where(a => a.QuestionId == questionId);
            var answerDtos = _mapper.Map<IEnumerable<AnswerDto>>(questionAnswers);
            return Result<IEnumerable<AnswerDto>>.Success(answerDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Soru cevapları getirilirken hata oluştu: {QuestionId}", questionId);
            return Result<IEnumerable<AnswerDto>>.Failure(Error.Create(ErrorCode.InternalServerError, "Soru cevapları getirilirken hata oluştu"));
        }
    }

    public async Task<Result<AnswerDto>> CreateAnswerAsync(CreateAnswerDto createAnswerDto)
    {
        try
        {
            // Soru kontrolü
            var question = await _questionRepository.GetByIdAsync(createAnswerDto.QuestionId);
            if (question == null)
            {
                return Result<AnswerDto>.Failure(Error.Create(ErrorCode.NotFound, "Soru bulunamadı"));
            }

            // Kullanıcı kontrolü
            var user = await _userRepository.GetByIdAsync(createAnswerDto.UserId);
            if (user == null)
            {
                return Result<AnswerDto>.Failure(Error.Create(ErrorCode.NotFound, "Kullanıcı bulunamadı"));
            }

            var answer = _mapper.Map<Answer>(createAnswerDto);
            answer.CreatedAt = DateTime.UtcNow;
            answer.IsActive = true;

            await _answerRepository.AddAsync(answer);
            await _answerRepository.SaveChangesAsync();

            var answerDto = _mapper.Map<AnswerDto>(answer);
            return Result<AnswerDto>.Success(answerDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap oluşturulurken hata oluştu");
            return Result<AnswerDto>.Failure(Error.Create(ErrorCode.InternalServerError, "Cevap oluşturulurken hata oluştu"));
        }
    }

    public async Task<Result<AnswerDto>> UpdateAnswerAsync(int id, UpdateAnswerDto updateAnswerDto)
    {
        try
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Result<AnswerDto>.Failure(Error.Create(ErrorCode.NotFound, "Cevap bulunamadı"));
            }

            answer.Content = updateAnswerDto.Content;
            answer.UpdatedAt = DateTime.UtcNow;

            await _answerRepository.UpdateAsync(answer);
            await _answerRepository.SaveChangesAsync();

            var answerDto = _mapper.Map<AnswerDto>(answer);
            return Result<AnswerDto>.Success(answerDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap güncellenirken hata oluştu: {AnswerId}", id);
            return Result<AnswerDto>.Failure(Error.Create(ErrorCode.InternalServerError, "Cevap güncellenirken hata oluştu"));
        }
    }

    public async Task<Result<bool>> DeleteAnswerAsync(int id)
    {
        try
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Result<bool>.Failure(Error.Create(ErrorCode.NotFound, "Cevap bulunamadı"));
            }

            await _answerRepository.DeleteAsync(answer);
            await _answerRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap silinirken hata oluştu: {AnswerId}", id);
            return Result<bool>.Failure(Error.Create(ErrorCode.InternalServerError, "Cevap silinirken hata oluştu"));
        }
    }

    public async Task<Result<bool>> ActivateAnswerAsync(int id)
    {
        try
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Result<bool>.Failure(Error.Create(ErrorCode.NotFound, "Cevap bulunamadı"));
            }

            answer.IsActive = true;
            answer.UpdatedAt = DateTime.UtcNow;

            await _answerRepository.UpdateAsync(answer);
            await _answerRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap aktifleştirilirken hata oluştu: {AnswerId}", id);
            return Result<bool>.Failure(Error.Create(ErrorCode.InternalServerError, "Cevap aktifleştirilirken hata oluştu"));
        }
    }

    public async Task<Result<bool>> DeactivateAnswerAsync(int id)
    {
        try
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return Result<bool>.Failure(Error.Create(ErrorCode.NotFound, "Cevap bulunamadı"));
            }

            answer.IsActive = false;
            answer.UpdatedAt = DateTime.UtcNow;

            await _answerRepository.UpdateAsync(answer);
            await _answerRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cevap deaktifleştirilirken hata oluştu: {AnswerId}", id);
            return Result<bool>.Failure(Error.Create(ErrorCode.InternalServerError, "Cevap deaktifleştirilirken hata oluştu"));
        }
    }
}
