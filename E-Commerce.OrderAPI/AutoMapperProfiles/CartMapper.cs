using AutoMapper;
using E_Commerce.OrderAPI.Models;
using E_Commerce.OrderAPI.Models.Dto.Cart;
using System.Data;

namespace E_Commerce.OrderAPI.AutoMapperProfiles
{
    public class CartMapper : Profile
    {
        public CartMapper()
        {
            CreateMap<CartDto, Cart>().ReverseMap();
        }
    }
}
