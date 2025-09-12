namespace BAMK.Core.Entities
{
    public abstract class AuditableEntity : BaseEntity
    {
        public new DateTime CreatedAt { get; set; }
        public new DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
