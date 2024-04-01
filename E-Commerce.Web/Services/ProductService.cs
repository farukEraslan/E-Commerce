using E_Commerce.Web.Models;
using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Product;
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

        // ilerde kullanılabilir diye yazıldı.
        public async Task<ResponseDto> TotalProductSize()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + $"/api/product/total-product"
            });
        }

        public async Task<ResponseDto> GetAllAsync(int page, int size)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + $"/api/product/list/page={page}&size={size}"
            });
        }

        public async Task<ResponseDto> GetByIdAsync(Guid productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product/getById/" + productId
            });
        }

        public async Task<ResponseDto> CreateAsync(ProductCreateDto productCreateDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = productCreateDto,
                Url = SD.ProductAPIBase + "/api/product/create"
            });
        }

        public async Task<ResponseDto> DeleteAsync(Guid productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + $"/api/product/delete/" + productId
            });
        }

        public async Task<ResponseDto> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.PUT,
                Data = productUpdateDto,
                Url = SD.ProductAPIBase + "/api/product/update"
            });
        }
    }
}
