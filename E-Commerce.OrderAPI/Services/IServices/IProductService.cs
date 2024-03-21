using E_Commerce.OrderAPI.Models.Dto.Product;

namespace E_Commerce.OrderAPI.Services.IServices
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts();
        Task<ProductDto> GetById(Guid productId);
        Task IncreaseProductStock(Guid productId);
        Task DecreaseProductStock(Guid productId);
    }
}
