using E_Commerce.Web.Models;
using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Services.IServices;
using E_Commerce.Web.Utility;

namespace E_Commerce.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public Task<ResponseDto> CreateAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> DeleteAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> GetAllAsync()
        
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product/list"
            });
        }

        public Task<ResponseDto> GetByIdAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
