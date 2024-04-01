using E_Commerce.Web.Models;
using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Category;
using E_Commerce.Web.Services.IServices;
using E_Commerce.Web.Utility;

namespace E_Commerce.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private IBaseService _baseService;

        public CategoryService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateAsync(CategoryDto categoryDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = categoryDto,
                Url = SD.ProductAPIBase + "/api/category/create"
            });
        }

        public async Task<ResponseDto> DeleteAsync(Guid categoryId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/category/delete/" + categoryId
            });
        }

        public async Task<ResponseDto> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + $"/api/category/get-all"
            });
        }

        public async Task<ResponseDto> GetAllAsync(int page, int size)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + $"/api/category/list/page={page}&size={size}"
            });
        }

        public async Task<ResponseDto> GetByIdAsync(Guid categoryId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/category/getById/" + categoryId
            });
        }

        public async Task<ResponseDto> UpdateAsync(CategoryUpdateDto categoryDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.PUT,
                Data = categoryDto,
                Url = SD.ProductAPIBase + "/api/category/update"
            });
        }
    }
}
