using E_Commerce.Web.Dto.Request;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace E_Commerce.Web.Validation.FluentValidation
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            // FirstName Rules
            RuleFor(x => x.FirstName).NotNull().WithMessage("İsim kısmı boş olamaz.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim kısmı boş olamaz.");
            RuleFor(x => x.FirstName).MinimumLength(2).WithMessage("En az 2 harfli bir isim giriniz.");

            // LastName Rules
            RuleFor(x => x.LastName).NotNull().WithMessage("Soyisim kısmı boş olamaz.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyisim kısmı boş olamaz.");
            RuleFor(x => x.LastName).MinimumLength(2).WithMessage("En az 2 harfli bir soyisim giriniz.");

            // Email Rules
            RuleFor(x => x.Email).NotNull().WithMessage("Email kısmı boş olamaz.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email kısmı boş olamaz.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email kısmı boş olamaz.");

            // Password Rules
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş olamaz.");
            RuleFor(x => x.Password).Length(8, 16).WithMessage("Şifre en az 8 en fazla 16 karakter uzunluğunda olmalıdır.");
            RuleFor(x => x.Password).Matches("[A-Z]").WithMessage("Şifrede en az bir büyük harf olmalıdır.");
            RuleFor(x => x.Password).Matches("[a-z]").WithMessage("Şifrede en az bir küçük harf olmalıdır.");
            RuleFor(x => x.Password).Matches("[0-9]").WithMessage("Şifrede en az bir sayı olmalıdır.");
            RuleFor(x => x.Password).Matches("[!@#$%^&*()-+=]").WithMessage("Şifrede en az bir özel karakter olmalıdır.");

            // PhoneNumber Rules
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage("Telefon numarası kısmı boş olamaz.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon numarası kısmı boş olamaz.");
            RuleFor(x => x.PhoneNumber).Must(PhoneNumberCheck).WithMessage("Geçerli bir telefon numarası giriniz.");

            // BirthDate Rules
            RuleFor(x => x.BirthDate).NotNull().WithMessage("Doğum günü kısmı boş olamaz.");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Doğum günü kısmı boş olamaz.");
            RuleFor(x => x.BirthDate).Must(BirthDateCheck).WithMessage("Üye olabilmek için 18 yaşından büyük olmalısınız.");
        }

        private bool PhoneNumberCheck(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^[1-9]\d{9}$");
        }

        private bool BirthDateCheck(DateTime birthDate)
        {
            return birthDate < DateTime.Now.AddYears(-18) ? true : false;
        }
    }
}
