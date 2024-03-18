using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Product;

namespace E_Commerce.Web.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDto> CreateAsync(ProductCreateDto productDto);
        Task<ResponseDto> UpdateAsync(ProductUpdateDto productDto);
        Task<ResponseDto> DeleteAsync(Guid productId);
        Task<ResponseDto> GetAllAsync(int page, int size);
        Task<ResponseDto> GetByIdAsync(Guid productId);
    }
}
