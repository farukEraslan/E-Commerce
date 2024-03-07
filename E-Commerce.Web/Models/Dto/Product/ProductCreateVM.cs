using E_Commerce.Web.Models.Dto.Category;

namespace E_Commerce.Web.Models.Dto.Product
{
    public class ProductCreateVM
    {
        public ProductCreateDto ProductCreateDto { get; set; }
        public List<CategoryDto> CategoryDto { get; set; } = new List<CategoryDto>();
    }
}
