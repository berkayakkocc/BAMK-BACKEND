using BAMK.Core.Entities;

namespace BAMK.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int TShirtId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual TShirt TShirt { get; set; } = null!;
    }
}