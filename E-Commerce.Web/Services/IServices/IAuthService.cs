using E_Commerce.Web.Dto.Request;
using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Request;

namespace E_Commerce.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<ResponseDto> ActivateUser(string userEmail);
        Task LogoutAsync();
    }
}
