using E_Commerce.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models.Dto;

namespace E_Commerce.ProductAPI.Services.IServices
{
    public interface IProductService
    {
        ResponseDto Create(ProductCreateDto productCreateDto);
        ResponseDto Update(ProductDto productDto);
        ResponseDto Delete(Guid productId);
        ResponseDto GetAll();
        ResponseDto GetById(Guid productId);
    }
}
