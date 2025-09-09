namespace BAMK.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string? ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TShirtId { get; set; }
        public string TShirtName { get; set; } = string.Empty;
        public string TShirtColor { get; set; } = string.Empty;
        public string TShirtSize { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public string? ShippingAddress { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public int TShirtId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class UpdatePaymentStatusDto
    {
        public string PaymentStatus { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
    }
}
