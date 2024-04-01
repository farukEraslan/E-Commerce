using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Category;

namespace E_Commerce.Web.Services.IServices
{
    public interface ICategoryService
    {
        Task<ResponseDto> CreateAsync(CategoryDto categoryDto);
        Task<ResponseDto> UpdateAsync(CategoryUpdateDto categoryDto);
        Task<ResponseDto> DeleteAsync(Guid categoryId);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetAllAsync(int page, int size);
        Task<ResponseDto> GetByIdAsync(Guid categoryId);
    }
}
