using E_Commerce.Web.Models;
using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Order;
using E_Commerce.Web.Services.IServices;
using E_Commerce.Web.Utility;

namespace E_Commerce.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> AddToCart(CreateCartDto createCartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = createCartDto,
                Url = SD.OrderAPIBase + "/api/order/add-to-cart"
            });
        }

        public async Task<ResponseDto> RemoveFromCart(RemoveCartDto removeCartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = removeCartDto,
                Url = SD.OrderAPIBase + "/api/order/remove-from-cart"
            });
        }

        public async Task<ResponseDto> GetCart(Guid userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.OrderAPIBase + "/api/order/get-cart/" + userId
            });
        }

        public async Task<ResponseDto> GiveOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.PUT,
                Data = cartDto,
                Url = SD.OrderAPIBase + "/api/order/give-order"
            });
        }

        public async Task<ResponseDto> GetOrders()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.OrderAPIBase + "/api/order/get-orders"
            });
        }

        public async Task<ResponseDto> ApproveOrder(Guid cartId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.PUT,
                Url = SD.OrderAPIBase + "/api/order/approve-order"
            });
        }
    }
}
