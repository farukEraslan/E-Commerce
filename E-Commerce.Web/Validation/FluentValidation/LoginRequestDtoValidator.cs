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
        }
    }
}
