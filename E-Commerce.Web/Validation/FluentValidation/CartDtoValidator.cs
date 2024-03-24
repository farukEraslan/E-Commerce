using E_Commerce.Web.Models.Dto.Order;
using FluentValidation;

namespace E_Commerce.Web.Validation.FluentValidation
{
    public class CartDtoValidator : AbstractValidator<CartDto>
    {
        public CartDtoValidator()
        {
            RuleFor(x=>x.Address).NotEmpty();
            RuleFor(x=>x.Address).NotNull();
            RuleFor(x=>x.Address).MinimumLength(20);
        }
    }
}
