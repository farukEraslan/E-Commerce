using AutoMapper;
using E_Commerce.ShoppingCartAPI.Models;
using E_Commerce.ShoppingCartAPI.Models.Dto;

namespace E_Commerce.ShoppingCartAPI.AutoMapperProfiles
{
    public class CartDetailsMapper : Profile
    {
        public CartDetailsMapper()
        {
            CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
        }
    }
}
