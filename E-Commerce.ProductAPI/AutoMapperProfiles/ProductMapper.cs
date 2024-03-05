using AutoMapper;
using E_Commerce.ProductAPI.Models;
using E_Commerce.ProductAPI.Models.Dto;

namespace E_Commerce.ProductAPI.AutoMapperProfiles
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryName))
                .ReverseMap();
        }
    }
}
