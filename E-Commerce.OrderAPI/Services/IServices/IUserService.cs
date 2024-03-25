using E_Commerce.OrderAPI.Models.Dto;

namespace E_Commerce.OrderAPI.Services.IServices
{
    public interface IUserService
    {
        Task<UserDto> GetById(Guid userId);
    }
}
