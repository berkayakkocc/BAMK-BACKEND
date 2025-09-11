namespace BAMK.Application.DTOs.ProductDetail
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public int TShirtId { get; set; }
        public string? Material { get; set; }
        public string? CareInstructions { get; set; }
        public string? Brand { get; set; }
        public string? Origin { get; set; }
        public string? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? Features { get; set; }
        public string? AdditionalInfo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateProductDetailDto
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
    }

    public class UpdateProductDetailDto
    {
        public string? Material { get; set; }
        public string? CareInstructions { get; set; }
        public string? Brand { get; set; }
        public string? Origin { get; set; }
        public string? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? Features { get; set; }
        public string? AdditionalInfo { get; set; }
        public bool IsActive { get; set; }
    }
}
