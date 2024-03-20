using E_Commerce.Web.Models.Dto.Product;
using FluentValidation;

namespace E_Commerce.Web.Validation.FluentValidation
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            // ProductName 
            RuleFor(x=>x.ProductName).NotEmpty();
            RuleFor(x=>x.ProductName).NotNull();

            // UnitPrice
            RuleFor(x=>x.UnitPrice).NotEmpty();
            RuleFor(x=>x.UnitPrice).NotNull();
            RuleFor(x=>x.UnitPrice).GreaterThan(0);
            RuleFor(x=>x.UnitPrice).LessThanOrEqualTo(10000);

            // StockAmount
            RuleFor(x=>x.StockAmount).NotEmpty();
            RuleFor(x=>x.StockAmount).NotNull();
            RuleFor(x=>x.StockAmount).InclusiveBetween(1,100);

            // ISBN
            RuleFor(x=>x.ISBN).NotEmpty();
            RuleFor(x=>x.ISBN).NotNull();
            RuleFor(x=>x.ISBN).Length(13);

            // CategoryId
            RuleFor(x=>x.CategoryId).NotEmpty();
            RuleFor(x=>x.CategoryId).NotNull();
        }
    }
}
