using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Order;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> AddToCartOnHomePage(Guid productId)
        {
            var userId = HttpContext.User.Claims.ToList()[1].Value;

            CreateCartDto createCartDto = new CreateCartDto();
            createCartDto.ProductId = productId;
            createCartDto.UserId = Guid.Parse(userId);

            var result = await _orderService.AddToCart(createCartDto);
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("Cart", "Order", $"{userId}");
        }

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> AddToCartOnCart(Guid productId)
        {
            var userId = HttpContext.User.Claims.ToList()[1].Value;

            CreateCartDto createCartDto = new CreateCartDto();
            createCartDto.ProductId = productId;
            createCartDto.UserId = Guid.Parse(userId);

            var result = await _orderService.AddToCart(createCartDto);
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Cart", "Order", $"{userId}");
        }

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> RemoveFromCartOnHomePage(Guid productId)
        {
            var userId = HttpContext.User.Claims.ToList()[1].Value;

            RemoveCartDto removeCartDto = new RemoveCartDto();
            removeCartDto.ProductId = productId;
            removeCartDto.UserId = Guid.Parse(userId);

            var result = await _orderService.RemoveFromCart(removeCartDto);
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("Cart", "Order", $"{userId}");
        }

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> RemoveFromCartOnCart(Guid productId)
        {
            var userId = HttpContext.User.Claims.ToList()[1].Value;

            RemoveCartDto removeCartDto = new RemoveCartDto();
            removeCartDto.ProductId = productId;
            removeCartDto.UserId = Guid.Parse(userId);

            var result = await _orderService.RemoveFromCart(removeCartDto);
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Cart", "Order", $"{userId}");
        }

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> Cart()
        {
            var userId = HttpContext.User.Claims.ToList()[1].Value;

            CartDto cartDto = new();
            ResponseDto? cartResponse = await _orderService.GetCart(Guid.Parse(userId));

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

        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GiveOrder(CartDto cartDto)
        {
            var userId = HttpContext.User.Claims.ToList()[1].Value;

            ResponseDto response = await _orderService.GetCart(Guid.Parse(userId));
            var cart = JsonConvert.DeserializeObject<CartDto>(response.Result.ToString());
            if (cart != null)
            {
                cart.Address = cartDto.Address;
                cart.IsCompleted = true;
                await _orderService.GiveOrder(cart);
            }
            return RedirectToAction("Cart", "Order");
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrder(Guid cartId)
        {
            ResponseDto result = await _orderService.DeleteOrder(cartId);
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
