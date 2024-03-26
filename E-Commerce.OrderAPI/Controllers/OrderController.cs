using E_Commerce.OrderAPI.Models.Dto.Cart;
using E_Commerce.OrderAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.OrderAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICartService _cartService;

        public OrderController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpPost("add-to-cart")]
        [Authorize(Roles = "customer")]
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
        [Authorize(Roles = "customer")]
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
        [Authorize(Roles = "customer")]
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

        [HttpPut("give-order")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GiveOrder([FromBody] CartDto cartDto)
        {
            var result = await _cartService.GiveOrder(cartDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("get-orders")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _cartService.GetOrders();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("approve-order/{cartId:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveOrder([FromRoute] Guid cartId)
        {
            var result = await _cartService.ApproveOrder(cartId);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpDelete("delete-order/{cartId:guid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid cartId)
        {
            var result = await _cartService.DeleteOrder(cartId);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
