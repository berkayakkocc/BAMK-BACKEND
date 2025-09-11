using BAMK.Core.Entities;

namespace BAMK.Domain.Entities
{
    public class ProductDetail : BaseEntity
    {
        public int TShirtId { get; set; }
        public string? Material { get; set; }
        public string? CareInstructions { get; set; }
        public string? Brand { get; set; }
        public string? Origin { get; set; }
        public string? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? Features { get; set; }
        public string? AdditionalInfo { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual TShirt TShirt { get; set; } = null!;
    }
}

