using BAMK.Application.DTOs.Order;
using FluentValidation;

namespace BAMK.Application.Validation
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Kullanıcı ID geçersiz");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("Teslimat adresi gereklidir")
                .MaximumLength(500).WithMessage("Teslimat adresi 500 karakterden fazla olamaz")
                .MinimumLength(10).WithMessage("Teslimat adresi en az 10 karakter olmalıdır");

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Sipariş en az bir ürün içermelidir")
                .Must(items => items != null && items.Any()).WithMessage("Sipariş öğeleri boş olamaz");

            RuleForEach(x => x.OrderItems)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.TShirtId)
                        .GreaterThan(0).WithMessage("Ürün ID geçersiz");
                    
                    item.RuleFor(x => x.Quantity)
                        .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır")
                        .LessThanOrEqualTo(100).WithMessage("Miktar 100'den fazla olamaz");
                });
        }
    }
}
