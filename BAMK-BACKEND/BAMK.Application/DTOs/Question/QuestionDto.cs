using BAMK.Core.Common;

namespace BAMK.Application.DTOs.Question;

public class QuestionDto
{
    public int Id { get; set; }
    public string QuestionTitle { get; set; } = string.Empty;
    public string QuestionContent { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public List<AnswerDto> Answers { get; set; } = new();
}

public class CreateQuestionDto
{
    public string QuestionTitle { get; set; } = string.Empty;
    public string QuestionContent { get; set; } = string.Empty;
    public int UserId { get; set; }
}

public class UpdateQuestionDto
{
    public string QuestionTitle { get; set; } = string.Empty;
    public string QuestionContent { get; set; } = string.Empty;
}

public class AnswerDto
{
    public int Id { get; set; }
    public string AnswerContent { get; set; } = string.Empty;
    public int QuestionId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class CreateAnswerDto
{
    public string AnswerContent { get; set; } = string.Empty;
    public int QuestionId { get; set; }
    public int UserId { get; set; }
}

public class UpdateAnswerDto
{
    public string AnswerContent { get; set; } = string.Empty;
}
