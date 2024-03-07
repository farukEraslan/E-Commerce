using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Product;

namespace E_Commerce.Web.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDto> CreateAsync(ProductCreateDto productDto);
        Task<ResponseDto> UpdateAsync(ProductUpdateDto productDto);
        Task<ResponseDto> DeleteAsync(ProductDto productDto);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetByIdAsync(ProductDto productDto);
    }
}
