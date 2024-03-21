using AutoMapper;
using E_Commerce.ShoppingCartAPI.Data;
using E_Commerce.ShoppingCartAPI.Models;
using E_Commerce.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace E_Commerce.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public ShoppingCartController(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _response = new ResponseDto();
        }

        [HttpPost("cart-upsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            var cartHeaderFromDb = _appDbContext.CartHeaders.FirstOrDefault(user => user.UserId == cartDto.CartHeader.UserId);
            if (cartHeaderFromDb == null)
            {
                // create header and details
                CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                _appDbContext.CartHeaders.Add(cartHeader);
                await _appDbContext.SaveChangesAsync ();
                cartDto.CartDetails.First().CartHeaderId = cartHeader.Id;
                _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                // check details has same product
                var cartDetailsFromDb = _appDbContext.CartDetails.FirstOrDefault(detail => detail.ProductId == cartDto.CartDetails.First().ProductId && detail.CartHeaderId == cartHeaderFromDb.Id);
                if (cartDetailsFromDb == null)
                {
                    // create cart details
                    cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                    _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    // update quantity in cart details
                    cartDto.CartDetails.First().Quantity += cartDetailsFromDb.Quantity;
                    cartDto.CartDetails.First().CartHeaderId = Guid.NewGuid();
                    cartDto.CartDetails.First().Id = Guid.NewGuid();
                }

                _response.Result = cartDto;
            }

            return _response;
        }
    }
}
