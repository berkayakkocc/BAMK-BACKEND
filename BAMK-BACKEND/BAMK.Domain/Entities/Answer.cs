using BAMK.Core.Entities;

namespace BAMK.Domain.Entities
{
    public class Answer : BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public bool IsAccepted { get; set; } = false;

        // Navigation properties
        public virtual Question Question { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}