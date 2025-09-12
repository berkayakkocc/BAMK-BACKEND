using BAMK.Application.DTOs.Question;
using FluentValidation;

namespace BAMK.Application.Validation
{
    public class CreateQuestionDtoValidator : AbstractValidator<CreateQuestionDto>
    {
        public CreateQuestionDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Kullanıcı ID geçersiz");

            RuleFor(x => x.QuestionTitle)
                .NotEmpty().WithMessage("Soru başlığı gereklidir")
                .MaximumLength(200).WithMessage("Soru başlığı 200 karakterden fazla olamaz")
                .MinimumLength(5).WithMessage("Soru başlığı en az 5 karakter olmalıdır");

            RuleFor(x => x.QuestionContent)
                .NotEmpty().WithMessage("Soru içeriği gereklidir")
                .MaximumLength(2000).WithMessage("Soru içeriği 2000 karakterden fazla olamaz")
                .MinimumLength(10).WithMessage("Soru içeriği en az 10 karakter olmalıdır");
        }
    }

    public class CreateAnswerDtoValidator : AbstractValidator<CreateAnswerDto>
    {
        public CreateAnswerDtoValidator()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("Soru ID geçersiz");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Kullanıcı ID geçersiz");

            RuleFor(x => x.AnswerContent)
                .NotEmpty().WithMessage("Cevap içeriği gereklidir")
                .MaximumLength(2000).WithMessage("Cevap içeriği 2000 karakterden fazla olamaz")
                .MinimumLength(5).WithMessage("Cevap içeriği en az 5 karakter olmalıdır");
        }
    }
}
