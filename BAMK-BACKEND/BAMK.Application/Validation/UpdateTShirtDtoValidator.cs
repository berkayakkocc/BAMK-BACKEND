using BAMK.Application.DTOs.TShirt;
using FluentValidation;

namespace BAMK.Application.Validation
{
    public class UpdateTShirtDtoValidator : AbstractValidator<UpdateTShirtDto>
    {
        public UpdateTShirtDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı gereklidir")
                .MaximumLength(200).WithMessage("Ürün adı 200 karakterden fazla olamaz")
                .MinimumLength(2).WithMessage("Ürün adı en az 2 karakter olmalıdır");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(10000).WithMessage("Fiyat 10.000'den fazla olamaz");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz")
                .LessThanOrEqualTo(10000).WithMessage("Stok miktarı 10.000'den fazla olamaz");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Açıklama 1000 karakterden fazla olamaz")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("Resim URL'si 500 karakterden fazla olamaz")
                .Must(BeAValidUrl).WithMessage("Geçerli bir URL formatı giriniz")
                .When(x => !string.IsNullOrEmpty(x.ImageUrl));

            RuleFor(x => x.Color)
                .MaximumLength(50).WithMessage("Renk 50 karakterden fazla olamaz")
                .When(x => !string.IsNullOrEmpty(x.Color));

            RuleFor(x => x.Size)
                .MaximumLength(20).WithMessage("Beden 20 karakterden fazla olamaz")
                .When(x => !string.IsNullOrEmpty(x.Size));
        }

        private static bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
    }
}
