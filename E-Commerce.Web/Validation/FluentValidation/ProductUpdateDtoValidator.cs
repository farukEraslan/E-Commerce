using E_Commerce.Web.Models.Dto.Product;
using FluentValidation;

namespace E_Commerce.Web.Validation.FluentValidation
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            // ProductName 
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.ProductName).NotNull();

            // CategoryId
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.CategoryId).NotNull();

            // UnitPrice
            RuleFor(x => x.UnitPrice).NotEmpty();
            RuleFor(x => x.UnitPrice).NotNull();
            RuleFor(x => x.UnitPrice).InclusiveBetween(1, 10000);

            // StockAmount
            RuleFor(x => x.StockAmount).NotEmpty();
            RuleFor(x => x.StockAmount).NotNull();
            RuleFor(x => x.StockAmount).GreaterThan(0);
            RuleFor(x => x.StockAmount).LessThanOrEqualTo(100);

            // ISBN
            RuleFor(x => x.ISBN).NotEmpty();
            RuleFor(x => x.ISBN).NotNull();
            RuleFor(x => x.ISBN).Length(13);
        }
    }
}
