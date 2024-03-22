using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Order;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace E_Commerce.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> AddToCart(Guid productId, Guid userId)
        {
            CreateCartDto createCartDto = new CreateCartDto();
            createCartDto.ProductId = productId;
            createCartDto.UserId = userId;

            var result = await _orderService.AddToCart(createCartDto);
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("Cart", "Order", $"{userId}");
        }

        public async Task<IActionResult> RemoveFromCart(Guid productId, Guid userId)
        {
            RemoveCartDto removeCartDto = new RemoveCartDto();
            removeCartDto.ProductId = productId;
            removeCartDto.UserId = userId;

            var result = await _orderService.RemoveFromCart(removeCartDto);
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("Cart", "Order", $"{userId}");
        }

        public async Task<IActionResult> Cart(Guid userId)
        {
            CartDto cartDto = new();
            ResponseDto? cartResponse = await _orderService.GetCart(userId);

            if (cartResponse != null && cartResponse.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(cartResponse.Result));
                return View(cartDto);
            }
            else
            {
                TempData["error"] = cartResponse?.Message;
                return View(cartDto);
            }

        }
    }
}
