using AutoMapper;
using E_Commerce.OrderAPI.Data;
using E_Commerce.OrderAPI.Models;
using E_Commerce.OrderAPI.Models.Dto;
using E_Commerce.OrderAPI.Models.Dto.Cart;
using E_Commerce.OrderAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.OrderAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        private readonly AppDbContext _appDbContext;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public CartService(IProductService productService, AppDbContext appDbContext, IMapper mapper)
        {
            _productService = productService;
            _appDbContext = appDbContext;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddtoCart(Guid productId, Guid userId)
        {
            var cart = _appDbContext.Carts.FirstOrDefault(cart => cart.UserId == userId && cart.IsCompleted == false);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CartTotalPrice = 0,
                };
                _appDbContext.Carts.Add(cart);
                await _appDbContext.SaveChangesAsync();
            }

            var product = await _productService.GetById(productId);

            var cartLine = _appDbContext.CartLines.FirstOrDefault(x => x.ProductId == productId && x.CartId == cart.Id);

            if (cartLine == null)
            {
                var newCartLine = new CartLine
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1,
                    CartLinePrice = product.UnitPrice
                };

                // sepete eklenen ürünün stok miktarı 1 azaltılacak.
                await _productService.DecreaseProductStock(productId);

                _appDbContext.CartLines.Add(newCartLine);
                await _appDbContext.SaveChangesAsync();

                cart.CartTotalPrice += product.UnitPrice;
                _appDbContext.Carts.Update(cart);
                await _appDbContext.SaveChangesAsync();

                _response.Message = "Ürün başarı ile sepete eklendi";
                return _response;
            }
            else if (cartLine.Quantity > product.StockAmount)
            {
                _response.IsSuccess = false;
                _response.Message = "Stok miktarı yeterli değil";
                return _response;
            }
            else if (cartLine.Quantity >= 10)
            {
                _response.IsSuccess = false;
                _response.Message = "En fazla 10 adet sipariş verebilirsiniz.";
                return _response;
            }
            else
            {
                cartLine.Quantity++;
                _appDbContext.Update(cartLine);
                await _appDbContext.SaveChangesAsync();

                // sepete eklenen ürünün stok miktarı 1 azaltılacak.
                await _productService.DecreaseProductStock(productId);

                cart.CartTotalPrice += product.UnitPrice;
                _appDbContext.Carts.Update(cart);
                await _appDbContext.SaveChangesAsync();

                _response.Message = "Ürün başarı ile sepete eklendi";
                return _response;
            }
        }

        public async Task<ResponseDto> RemoveFromCart(Guid productId, Guid userId)
        {
            var cart = _appDbContext.Carts.Where(cart => cart.UserId == userId && cart.IsCompleted == false).Include(x => x.CartLines).FirstOrDefault();

            var product = await _productService.GetById(productId);

            var cartLine = _appDbContext.CartLines.FirstOrDefault(x => x.ProductId == productId && x.CartId == cart.Id);

            if (cartLine.Quantity > 1)
            {
                cartLine.Quantity--;
                _appDbContext.Update(cartLine);
                await _appDbContext.SaveChangesAsync();

                // sepete eklenen ürün miktarı 1 arttırılacak.
                await _productService.IncreaseProductStock(productId);

                cart.CartTotalPrice -= product.UnitPrice;
                _appDbContext.Carts.Update(cart);
                await _appDbContext.SaveChangesAsync();

                _response.Message = "ürün adedi başarı ile azaltıldı.";
                return _response;
            }
            else
            {
                if (cart.CartLines.Count > 1)
                {
                    _appDbContext.CartLines.Remove(cartLine);

                    // sepete eklenen ürün miktarı 1 arttırılacak.
                    await _productService.IncreaseProductStock(productId);

                    cart.CartTotalPrice -= product.UnitPrice;
                    _appDbContext.Carts.Update(cart);
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    _appDbContext.CartLines.Remove(cartLine);
                    await _appDbContext.SaveChangesAsync();

                    // sepete eklenen ürün miktarı 1 arttırılacak.
                    await _productService.IncreaseProductStock(productId);

                    _appDbContext.Carts.Remove(cart);
                    await _appDbContext.SaveChangesAsync();
                }

                _response.Message = "Ürün başarı ile silindi.";
                return _response;
            }
        }

        public async Task<ResponseDto> GetCart(Guid userId)
        {
            // sorguya productta eklenecek.
            var cart = _appDbContext.Carts.Where(x => x.UserId == userId && x.IsCompleted == false).Include(x => x.CartLines).FirstOrDefault();

            if (cart == null)
            {
                _response.IsSuccess = false;
                return _response;
            }
            else
            {
                List<CartLineDto> cartLineDtos = new();
                foreach (var cartLine in cart.CartLines)
                {
                    var cartLineDto = _mapper.Map<CartLineDto>(cartLine);
                    var cartLineProduct = await _productService.GetById(cartLine.ProductId);
                    cartLineDto.Product = cartLineProduct;
                    cartLineDtos.Add(cartLineDto);
                }
                CartDto cartDto = _mapper.Map<CartDto>(cart);
                cartDto.CartLines = cartLineDtos;

                _response.Result = cartDto;
                _response.Message = "Sepet başarı ile listelendi.";
                return _response;
            }
        }

        public async Task<ResponseDto> GiveOrder(CartDto cartDto)
        {
            var existCart = _appDbContext.Carts.FirstOrDefault(cart => cart.Id == cartDto.Id);
            
            if (existCart != null)
            {
                _appDbContext.Carts.Update(_mapper.Map(cartDto, existCart));
                _appDbContext.SaveChanges();

                _response.Message = "Sipariş başarı şekilde verildi..";
                _response.Result = existCart;
                return _response;
            }
            else
            {
                _response.Message = "Sepet bulunamadı.";
                _response.IsSuccess = false;
                return _response;
            }
        }

        public async Task<ResponseDto> GetOrders()
        {
            _response.Result = _appDbContext.Carts.ToList();
            _response.Message = "Siparişler başarı ile listelendi.";
            return _response;
        }
        
        public async Task<ResponseDto> ApproveOrder(Guid cartId)
        {
            var cart = _appDbContext.Carts.FirstOrDefault(c => c.Id == cartId);
            if (cart != null)
            {
                cart.IsApproved = true;
                _appDbContext.Carts.Update(cart);
                _appDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = "Sipariş onaylandı.";
                
                // burada müşteriye sipariş onay maili gidecek.

                return _response;
            }
            else
            {
                _response.Message = "Sipariş onaylanırken bir hata oluştu.";
                return _response;
            }
        }
    }
}
