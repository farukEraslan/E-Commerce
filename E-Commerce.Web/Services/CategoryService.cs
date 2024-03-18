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

        public Task<ResponseDto> CreateAsync(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> DeleteAsync(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.CategoryAPIBase + "/api/category/list"
            });
        }

        public Task<ResponseDto> GetByIdAsync(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateAsync(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
