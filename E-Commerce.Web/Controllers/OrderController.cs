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

        public async Task<IActionResult> Index()
        {
            List<CartDto>? cartList = new();
            ResponseDto? orders = await _orderService.GetOrders();

            if (orders != null && orders.IsSuccess)
            {
                cartList = JsonConvert.DeserializeObject<List<CartDto>>(Convert.ToString(orders.Result));
                TempData["success"] = orders?.Message;
            }
            else
                TempData["error"] = orders?.Message;

            return View(cartList);
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
                if (!cartDto.IsCompleted)
                {
                    TempData["success"] = cartResponse?.Message;
                    return View(cartDto);
                }
                return View(new CartDto());
            }
            else
            {
                TempData["error"] = cartResponse?.Message;
                return View(cartDto);
            }

        }

        public async Task<IActionResult> GiveOrder(CartDto cartDto)
        {
            ResponseDto response = await _orderService.GetCart(cartDto.UserId);
            var cart = JsonConvert.DeserializeObject<CartDto>(response.Result.ToString());
            if (cart != null)
            {
                cart.Address = cartDto.Address;
                cart.IsCompleted = true;
                _orderService.GiveOrder(cart);
            }
            return RedirectToAction("Cart", "Order");
        }

        public async Task<IActionResult> ApproveOrder(Guid cartId)
        {
            ResponseDto result = await _orderService.ApproveOrder(cartId);
            if (result != null && result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index", "Order");
            }
            else
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", "Order");
            }
        }
    }
}
