using AutoMapper;
using E_Commerce.ProductAPI.Data;
using E_Commerce.ProductAPI.Models;
using E_Commerce.ProductAPI.Models.Dto;
using E_Commerce.ProductAPI.Models.Dto.Product;
using E_Commerce.ProductAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

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

        public int TotalProductNumber()
        {
            return _appDbContext.Products.Count();
        }
        public ResponseDto GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var productsWithCategories = _appDbContext.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(product => product.Category)
                    .Select(product => new ProductDto
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        CategoryName = product.Category != null ? product.Category.CategoryName : null, // Kategori adını alırken null kontrolü yapılmalıdır.
                        UnitPrice = product.UnitPrice,
                        StockAmount = product.StockAmount,
                        Author = product.Author,
                        Publisher = product.Publisher,
                        ISBN = product.ISBN,
                        ImageUrl = product.ImageUrl
                    })
                    .ToList();

                _response.Message = "Ürünler başarı ile listelendi.";
                _response.Result = productsWithCategories;
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
                var productWithCategory = _appDbContext.Products.Where(product => product.Id == productId)
                    .Include(product => product.Category)
                    .Select(product => new ProductDto
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.CategoryName,
                        UnitPrice = product.UnitPrice,
                        StockAmount = product.StockAmount,
                        Author = product.Author,
                        Publisher = product.Publisher,
                        ISBN = product.ISBN,
                        ImageUrl = product.ImageUrl
                    }).FirstOrDefault();

                _response.Message = "Ürün başarı ile listelendi.";
                _response.Result = productWithCategory;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }
        public ResponseDto Create(ProductCreateDto productCreateDto)
        {
            try
            {
                var hasProduct = _appDbContext.Products.FirstOrDefault(p => p.ISBN.Trim().ToUpper() == productCreateDto.ISBN.Trim().ToUpper());
                if (hasProduct != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Ürün zaten var.";
                    return _response;
                }

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
        public ResponseDto Update(ProductUpdateDto productUpdateDto)
        {
            try
            {
                var product = _appDbContext.Products.SingleOrDefault(p => p.Id == productUpdateDto.Id);
                var hasProduct = _appDbContext.Products.Where(p => p.Id != product.Id && p.ISBN == productUpdateDto.ISBN).FirstOrDefault();
                if (hasProduct != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Aynı ISBN numaralı iki ürün olamaz.";
                    return _response;
                }

                var result = _appDbContext.Products.Update(_mapper.Map(productUpdateDto, product));
                _appDbContext.SaveChanges();
                _response.Message = "Ürün başarı ile güncellendi.";
                _response.Result = productUpdateDto;
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
    }
}
