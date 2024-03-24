using E_Commerce.OrderAPI.Models.Dto.Cart;
using E_Commerce.OrderAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    //[Authorize(Roles = "customer")]
    public class OrderController : ControllerBase
    {
        private readonly ICartService _cartService;

        public OrderController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] CreateCartDto createCartDto)
        {
            var result = await _cartService.AddtoCart(createCartDto.ProductId,createCartDto.UserId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("remove-from-cart")]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveCartDto removeCartDto)
        {
            var result = await _cartService.RemoveFromCart(removeCartDto.ProductId, removeCartDto.UserId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("get-cart/{userId:guid}")]
        public async Task<IActionResult> GetCart([FromRoute] Guid userId)
        {
            var result = await _cartService.GetCart(userId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
