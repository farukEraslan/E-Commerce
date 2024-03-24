using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Order;
using E_Commerce.Web.Models.Dto.Order.ViewModels;

namespace E_Commerce.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto> AddToCart(CreateCartDto createCartDto);
        Task<ResponseDto> RemoveFromCart(RemoveCartDto removeCartDto);
        Task<ResponseDto> GetCart(Guid userId);
        Task<ResponseDto> GiveOrder(CartDto cartDto);
        Task<ResponseDto> GetOrders();
        Task<ResponseDto> ApproveOrder(Guid cartId);
    }
}
