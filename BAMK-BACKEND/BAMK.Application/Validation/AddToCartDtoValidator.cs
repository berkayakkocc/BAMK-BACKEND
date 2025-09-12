using BAMK.Application.DTOs.Cart;
using FluentValidation;

namespace BAMK.Application.Validation
{
    public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
    {
        public AddToCartDtoValidator()
        {
            RuleFor(x => x.TShirtId)
                .GreaterThan(0).WithMessage("Ürün ID geçersiz");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(50).WithMessage("Miktar 50'den fazla olamaz");
        }
    }

    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemDtoValidator()
        {
            RuleFor(x => x.CartItemId)
                .GreaterThan(0).WithMessage("Sepet öğesi ID geçersiz");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(50).WithMessage("Miktar 50'den fazla olamaz");
        }
    }
}
