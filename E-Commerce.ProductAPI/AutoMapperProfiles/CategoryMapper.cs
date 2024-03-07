using AutoMapper;
using E_Commerce.ProductAPI.Models;
using E_Commerce.ProductAPI.Models.Dto.Category;

namespace E_Commerce.ProductAPI.AutoMapperProfiles
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}
