namespace BAMK.Core.Entities
{
    public abstract class FullAuditableEntity : AuditableEntity
    {
        public new bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
