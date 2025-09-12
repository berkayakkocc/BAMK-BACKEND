namespace BAMK.Core.Entities
{
    public abstract class SoftDeleteEntity : BaseEntity
    {
        public new bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
