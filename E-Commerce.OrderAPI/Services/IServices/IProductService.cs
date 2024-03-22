using E_Commerce.OrderAPI.Models.Dto.Product;

namespace E_Commerce.OrderAPI.Services.IServices
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts();
        Task<ProductDto> GetById(Guid productId);
        Task<ProductDto> IncreaseProductStock(Guid productId);
        Task<ProductDto> DecreaseProductStock(Guid productId);
    }
}
