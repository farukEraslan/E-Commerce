using AutoMapper;
using E_Commerce.ProductAPI.Models;
using E_Commerce.ProductAPI.Models.Dto.Product;

namespace E_Commerce.ProductAPI.AutoMapperProfiles
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
