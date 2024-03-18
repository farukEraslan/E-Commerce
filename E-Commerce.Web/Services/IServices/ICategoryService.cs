using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Category;

namespace E_Commerce.Web.Services.IServices
{
    public interface ICategoryService
    {
        Task<ResponseDto> CreateAsync(CategoryDto categoryDto);
        Task<ResponseDto> UpdateAsync(CategoryDto categoryDto);
        Task<ResponseDto> DeleteAsync(CategoryDto categoryDto);
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto> GetByIdAsync(CategoryDto categoryDto);
    }
}
