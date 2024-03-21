using E_Commerce.OrderAPI.Models;
using E_Commerce.OrderAPI.Models.Dto;

namespace E_Commerce.OrderAPI.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto> AddtoCart(Guid productId, Guid userId);
        Task<ResponseDto> RemoveFromCart(Guid productId, Guid userId);
        ResponseDto GetCart(Guid userId);
    }
}
