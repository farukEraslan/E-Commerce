using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace E_Commerce.Web.Hubs
{
    public class SessionManagerHub : Hub
    {
        private readonly IAuthService _authService;

        public SessionManagerHub(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task ConnectionCheck(string jwtToken)
        {
            // *** Bağlanan tarayıcı için unique bir Id belirlenmesi gerekli.

            bool hasExistSession = false;
            var handler = new JwtSecurityTokenHandler();

            // token okuma işlemi
            if (jwtToken != null)
            {
                var jwt = handler.ReadJwtToken(jwtToken);
                var userId = jwt.Payload.Sub;

                if (ConnectedUser.UsersId.ContainsValue(userId))
                {
                    var existUser = ConnectedUser.UsersId.First(connectionId => connectionId.Value == userId);
                    // *** connectionId'si bilinen client'e hasExistSession bilgisi yollanacak.
                    hasExistSession = true;
                    // burada açık olan bağlantı kapatılacak.
                    await Clients.Client(existUser.Key).SendAsync("SignOut", hasExistSession);
                }
                else
                {
                    ConnectedUser.UsersId.Add(key: Context.ConnectionId, value: userId);
                    await Clients.Client(Context.ConnectionId).SendAsync("SignOut", hasExistSession);
                }
            }
        }

        #region LoginAsync
        /*

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
        */
        #endregion


    }
}
