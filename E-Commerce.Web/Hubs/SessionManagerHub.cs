using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Request;
using E_Commerce.Web.Models.Dto.Response;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce.Web.Hubs
{
    public class SessionManagerHub : Hub
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITokenProvider _tokenProvider;

        public SessionManagerHub(IAuthService authService, IHttpContextAccessor contextAccessor, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _contextAccessor = contextAccessor;
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> LoginAsync(string userName, string password)
        {
            LoginRequestDto loginRequestDto = new()
            {
                UserName = userName,
                Password = password
            };

            // giriş işlemi başarılı ile yapıldı.
            ResponseDto response = await _authService.LoginAsync(loginRequestDto);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                
                var handler = new JwtSecurityTokenHandler();

                // token okuma işlemi
                var jwt = handler.ReadJwtToken(loginResponseDto.Token);
                var userId = jwt.Payload.Sub;

                if (ConnectedUser.ConnectionId.ContainsValue(userId))
                {
                    // burada açık olan bağlantı kapatılacak. sonra yeni girişe izin verilecek
                    var existUserId = ConnectedUser.ConnectionId.First(connectionId => connectionId.Value == userId);
                    bool hasExistSession = true;
                    // *** connectionId'si bilinen client'e hasExistSession bilgisi yollanacak.
                    await Clients.Client(existUserId.Key).SendAsync("SessionCheck", hasExistSession);
                }
                ConnectedUser.ConnectionId.Add(key: Context.ConnectionId, value: userId);
                return loginResponseDto;
            }
            else
            {
                LoginResponseDto loginResponseDto = new()
                {
                    User = null,
                    Token = ""
                };
                return loginResponseDto;
            }
        }
    }
}
