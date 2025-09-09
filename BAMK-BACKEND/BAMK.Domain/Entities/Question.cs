using BAMK.Core.Entities;

namespace BAMK.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsAnswered { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}