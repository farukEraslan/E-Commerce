using E_Commerce.AuthAPI.Models.Dto;
using E_Commerce.AuthAPI.Models.Dto.Request;
using E_Commerce.AuthAPI.Models.Dto.Response;

namespace E_Commerce.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        /// <summary>
        /// Kullanıcı oluşturmak için kullanılır.
        /// </summary>
        /// <param name="registerationRequestDto"></param>
        /// <returns>Durum mesajı döner.</returns>
        Task<ResponseDto> Register(RegisterRequestDto registerationRequestDto);
        /// <summary>
        /// Kullanıcıya rol atamak için kullanılır.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns>İşlem başarılıysa True, işlem başarısızsa False döner.</returns>
        Task<bool> AssignRole(string email, string roleName);
        /// <summary>
        /// Kullanıcı girişi yapar.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns>LoginResponseDto tipinde bir cevap döner.</returns>
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
