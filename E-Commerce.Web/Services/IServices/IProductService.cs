using E_Commerce.Web.Models.Dto;

namespace E_Commerce.Web.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDto> CreateAsync(ProductDto productDto);
        Task<ResponseDto> UpdateAsync(ProductDto productDto);
        Task<ResponseDto> DeleteAsync(ProductDto productDto);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetByIdAsync(ProductDto productDto);
    }
}
