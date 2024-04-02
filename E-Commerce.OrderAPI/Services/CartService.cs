using AutoMapper;
using E_Commerce.OrderAPI.Data;
using E_Commerce.OrderAPI.Helpers;
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
        private readonly IUserService _userService;
        private readonly AppDbContext _appDbContext;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public CartService(IProductService productService, AppDbContext appDbContext, IMapper mapper, IUserService userService)
        {
            _productService = productService;
            _appDbContext = appDbContext;
            _response = new ResponseDto();
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ResponseDto> AddtoCart(Guid productId, Guid userId)
        {
            var cart = _appDbContext.Carts.FirstOrDefault(cart => cart.UserId == userId && cart.IsCompleted == false);
            var product = await _productService.GetById(productId);

            if (product.StockAmount > 0)
            {
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
            else
            {
                _response.Message = "Yeterli stok miktarı yok.";
                _response.IsSuccess = false;
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
            var cart = _appDbContext.Carts.Where(x => x.UserId == userId && x.IsCompleted == false).Include(x => x.CartLines).FirstOrDefault();

            if (cart == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Sepetinizde ürün bulunmamaktadır.";
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
                var updatedCart = _mapper.Map(cartDto, existCart);
                _appDbContext.Carts.Update(updatedCart);
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
            var orders = _mapper.Map<List<CartDto>>(_appDbContext.Carts.Include(x=>x.CartLines).ToList());
            foreach (var order in orders)
            {
                var user = await _userService.GetById(order.UserId);
                order.User = user;

                foreach (var cartLine in order.CartLines)
                {
                    cartLine.Product = await _productService.GetById(cartLine.ProductId);                    
                }
            }
            _response.Result = orders;
            _response.Message = "Siparişler başarı ile listelendi.";
            return _response;
        }

        public async Task<ResponseDto> ApproveOrder(Guid cartId)
        {
            var cart = _appDbContext.Carts.FirstOrDefault(c => c.Id == cartId);
            var user = await _userService.GetById(cart.UserId);
            if (cart != null)
            {
                cart.IsApproved = true;
                _appDbContext.Carts.Update(cart);
                _appDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = "Sipariş onaylandı.";

                // burada müşteriye sipariş onay maili gidecek.
                EmailSendHelper.SendEmailProducer(user.Email, cartId);    // burada hangfire implement edilebilir.
                return _response;
            }
            else
            {
                _response.Message = "Sipariş onaylanırken bir hata oluştu.";
                return _response;
            }
        }

        public async Task<ResponseDto> DeleteOrder(Guid cartId)
        {
            var cart = _appDbContext.Carts.FirstOrDefault(c => c.Id == cartId);
            var user = await _userService.GetById(cart.UserId);
            if (cart != null)
            {
                _appDbContext.Carts.Remove(cart);
                _appDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = "Sipariş onaylanmadı.";

                // burada müşteriye sipariş onay maili gidecek.
                EmailSendHelper.SendEmailProducer(user.Email, cartId);    // burada hangfire implement edilebilir.
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
