using BAMK.Core.Entities;

namespace BAMK.Domain.Entities
{
    public class Answer : BaseEntity
    {
        public string AnswerContent { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public bool IsAcceptedAnswer { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Question Question { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}