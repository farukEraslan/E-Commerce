using AutoMapper;
using E_Commerce.AuthAPI.Models;
using E_Commerce.AuthAPI.Models.Dto;
using E_Commerce.AuthAPI.Models.Dto.Request;

namespace E_Commerce.AuthAPI.AutoMapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<RegisterRequestDto, AppUser>();
            CreateMap<UserDto, AppUser>().ReverseMap();
        }
    }
}
