using E_Commerce.AuthAPI.Models.Dto;
using E_Commerce.AuthAPI.Models.Dto.Request;

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
        /// Kullanıcı girişi yapar.
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns>LoginResponseDto tipinde bir cevap döner.</returns>
        Task<ResponseDto> Login(LoginRequestDto loginRequestDto);

        /// <summary>
        /// Kullanıcıyı aktifleştirmek için kullanılır.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>Durum mesajı döner.</returns>
        Task<ResponseDto> UserActivate(string userEmail);

        /// <summary>
        /// Kullanıcıya rol atamak için kullanılır.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns>İşlem başarılıysa True, işlem başarısızsa False döner.</returns>
        Task<bool> AssignRole(string email, string roleName);

        /// <summary>
        /// Id'si verilen kullanıcıyı döndürür.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ResponseDto tipinde bir cevap döner.</returns>
        Task<ResponseDto> GetById(Guid userId);

        /// <summary>
        /// Müşterilerin email adreslerini getirir.
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> GetCustomerEmails();
    }
}
