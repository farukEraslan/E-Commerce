using E_Commerce.ProductAPI.Models.Dto;
using E_Commerce.ProductAPI.Models.Dto.Product;

namespace E_Commerce.ProductAPI.Services.IServices
{
    public interface IProductService
    {
        ResponseDto Create(ProductCreateDto productCreateDto);
        ResponseDto Update(ProductUpdateDto productUpdateDto);
        ResponseDto Delete(Guid productId);
        ResponseDto GetAll(int pageNumber, int pageSize);
        ResponseDto GetById(Guid productId);
        int TotalProductNumber();
    }
}
