using AutoMapper;
using E_Commerce.ProductAPI.Data;
using E_Commerce.ProductAPI.Models;
using E_Commerce.ProductAPI.Models.Dto;
using E_Commerce.ProductAPI.Services.IServices;
using Mango.Services.ProductAPI.Models.Dto;

namespace E_Commerce.ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public ProductService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        public ResponseDto Create(ProductCreateDto productCreateDto)
        {
            try
            {
                var result = _appDbContext.Products.Add(_mapper.Map<Product>(productCreateDto));
                _appDbContext.SaveChanges();
                _response.Message = "Ürün başarı ile eklendi.";
                _response.Result = productCreateDto;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto Delete(Guid productId)
        {
            try
            {
                var product = _appDbContext.Products.SingleOrDefault(p => p.Id == productId);
                var result = _appDbContext.Products.Remove(product);
                _appDbContext.SaveChanges();
                _response.Message = "Ürün başarı ile silindi.";
                _response.Result = product;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto GetAll()
        {
            try
            {
                var products = _appDbContext.Products.ToList();
                _response.Message = "Ürünler başarı ile listelendi.";
                _response.Result = products;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto GetById(Guid productId)
        {
            try
            {
                var product = _appDbContext.Products.SingleOrDefault(p => p.Id == productId);
                _response.Message = "Ürün başarı ile listelendi.";
                _response.Result = product;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        public ResponseDto Update(ProductDto productDto)
        {
            try
            {
                var product = _appDbContext.Products.SingleOrDefault(p => p.Id == productDto.Id);
                var result = _appDbContext.Products.Update(_mapper.Map(productDto, product));
                _appDbContext.SaveChanges();
                _response.Message = "Ürün başarı ile güncellendi.";
                _response.Result = productDto;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }
    }
}
