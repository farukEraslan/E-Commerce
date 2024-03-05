using E_Commerce.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models.Dto;

namespace E_Commerce.ProductAPI.Services.IServices
{
    public interface IProductService
    {
        ResponseDto Create(ProductDto productDto);
        ResponseDto Update(ProductDto productDto);
        ResponseDto Delete(Guid productId);
        ResponseDto GetAll(ProductDto productDto);
        ResponseDto GetById(Guid productId);
    }
}
