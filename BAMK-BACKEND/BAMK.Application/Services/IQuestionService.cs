using BAMK.Application.DTOs.Question;
using BAMK.Core.Common;

namespace BAMK.Application.Services;

public interface IQuestionService
{
    // Question CRUD
    Task<Result<QuestionDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<QuestionDto>>> GetAllAsync();
    Task<Result<IEnumerable<QuestionDto>>> GetByUserIdAsync(int userId);
    Task<Result<QuestionDto>> CreateAsync(CreateQuestionDto createQuestionDto);
    Task<Result<QuestionDto>> UpdateAsync(int id, UpdateQuestionDto updateQuestionDto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<bool>> ActivateAsync(int id);
    Task<Result<bool>> DeactivateAsync(int id);
    
    // Answer CRUD
    Task<Result<AnswerDto>> GetAnswerByIdAsync(int id);
    Task<Result<IEnumerable<AnswerDto>>> GetAnswersByQuestionIdAsync(int questionId);
    Task<Result<AnswerDto>> CreateAnswerAsync(CreateAnswerDto createAnswerDto);
    Task<Result<AnswerDto>> UpdateAnswerAsync(int id, UpdateAnswerDto updateAnswerDto);
    Task<Result<bool>> DeleteAnswerAsync(int id);
    Task<Result<bool>> ActivateAnswerAsync(int id);
    Task<Result<bool>> DeactivateAnswerAsync(int id);
}
