using E_Commerce.Web.Models.Dto.Request;
using FluentValidation;

namespace E_Commerce.Web.Validation.FluentValidation
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            // Email Rules
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Email kısmı boş olamaz.");
            RuleFor(x => x.UserName).NotNull().WithMessage("Email kısmı boş olamaz.");
            RuleFor(x => x.UserName).EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            // Password Rules
            RuleFor(x => x.Password).NotNull().WithMessage("Parola kısmı boş olamaz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola kısmı boş olamaz.");
            RuleFor(x => x.Password).MinimumLength(8).WithMessage("En az 8 karakterli bir parola giriniz.");
            RuleFor(x => x.Password).MaximumLength(16).WithMessage("En fazla 16 karakterli bir parola giriniz.");
        }
    }
}
