using E_Commerce.ProductAPI.Models.Dto;
using E_Commerce.ProductAPI.Models.Dto.Category;

namespace E_Commerce.ProductAPI.Services.IServices
{
    public interface ICategoryService
    {
        ResponseDto Create(CategoryCreateDto categoryCreateDto);
        ResponseDto Update(CategoryUpdateDto categoryUpdateDto);
        ResponseDto Delete(Guid categoryId);
        ResponseDto GetAll(int pageNumber, int pageSize);
        ResponseDto GetById(Guid categoryId);
    }
}
