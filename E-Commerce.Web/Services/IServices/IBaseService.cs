using E_Commerce.Web.Models;
using E_Commerce.Web.Models.Dto;

namespace E_Commerce.Web.Services.IServices
{
    public interface IBaseService
    {
        /// <summary>
        /// Bu metot gelne isteği ilgili HTTP Metoduna yollayarak gelen HTTP Response bilgisini döndürür.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="withBearer"></param>
        /// <returns></returns>
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
