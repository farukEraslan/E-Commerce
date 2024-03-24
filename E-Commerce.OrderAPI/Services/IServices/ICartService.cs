using E_Commerce.OrderAPI.Models;
using E_Commerce.OrderAPI.Models.Dto;
using E_Commerce.OrderAPI.Models.Dto.Cart;

namespace E_Commerce.OrderAPI.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto> AddtoCart(Guid productId, Guid userId);
        Task<ResponseDto> RemoveFromCart(Guid productId, Guid userId);
        Task<ResponseDto> GetCart(Guid userId);
        Task<ResponseDto> GiveOrder(CartDto cartDto);
        Task<ResponseDto> GetOrders();
        Task<ResponseDto> ApproveOrder(Guid cartId);
    }
}
