using E_Commerce.Web.Models.Dto.Category;

namespace E_Commerce.Web.Models.Dto.Product
{
    public class ProductUpdateVM
    {
        public ProductUpdateDto ProductUpdateDto { get; set; }
        public List<CategoryDto> CategoryDto { get; set; } = new List<CategoryDto>();
    }
}
