using AutoMapper;
using E_Commerce.ShoppingCartAPI.Models;
using E_Commerce.ShoppingCartAPI.Models.Dto;

namespace E_Commerce.ShoppingCartAPI.AutoMapperProfiles
{
    public class CartHeaderMapper : Profile
    {
        public CartHeaderMapper()
        {
            CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
        }
    }
}
