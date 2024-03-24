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

        /// <summary>
        /// Bağlanan kullanıcının browser adını kontrol ederek, değiştiği zaman önceki browser'ını logouta zorlayan metot.
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="browser"></param>
        /// <returns></returns>
        public async Task ConnectionCheck(string jwtToken, string browser)
        {
            if (jwtToken != null)
            {
                // token okuma işlemi
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(jwtToken);

                ConnectedUserInfo userInfo = new()
                {
                    Browser = browser,
                    UserId = jwt.Payload.Sub
                };

                ConnectedUser.UsersId.Add(key: Context.ConnectionId, value: userInfo);

                foreach (var item in ConnectedUser.UsersId.Values)
                {
                    if (browser != item.Browser && userInfo.UserId == item.UserId)
                    {
                        CloseConnections(item);
                    }
                }
                var test = ConnectedUser.UsersId;
            }
        }

        private async Task CloseConnections(ConnectedUserInfo item)
        {
            var connections = ConnectedUser.UsersId.Where(connection => connection.Value == item).ToList();
            foreach (var connection in connections)
            {
                await Clients.Client(connection.Key).SendAsync("SignOut", true);
                var connectedUserInfo = connection.Value;
                ConnectedUser.UsersId.Remove(key: connection.Key, value: out connectedUserInfo);
            }
        }

        #region ConnectionCheck
        /*
        public async Task ConnectionCheck(string jwtToken, decimal browserId)
        {
            // *** Bağlanan tarayıcı için unique bir Id belirlenmesi gerekli.

            bool hasExistSession = false;
            var handler = new JwtSecurityTokenHandler();

            // token okuma işlemi
            if (jwtToken != null)
            {
                var jwt = handler.ReadJwtToken(jwtToken);
                var userId = jwt.Payload.Sub;

                ConnectedUserInfo userInfo = new()
                {
                    BrowserId = browserId.ToString(),
                    UserId = jwt.Payload.Sub
                };

                if (ConnectedUser.UsersId.ContainsValue(userId))
                {
                    ConnectedUser.UsersId.Add(key: Context.ConnectionId, value: userId);
                    var clients = ConnectedUser.UsersId.ToList();

                    var userConnections = ConnectedUser.UsersId.Where(connectionId => connectionId.Value == userId).ToList();
                    // *** connectionId'si bilinen client'e hasExistSession bilgisi yollanacak.
                    hasExistSession = true;
                    // burada açık olan bağlantı kapatılacak.
                    foreach (var connection in userConnections)
                    {
                        await Clients.Client(connection.Key).SendAsync("SignOut", hasExistSession);
                    }
                    //await _authService.LogoutAsync();
                }
                else
                {
                    ConnectedUser.UsersId.Add(key: Context.ConnectionId, value: userId);
                    await Clients.Client(Context.ConnectionId).SendAsync("SignOut", hasExistSession);
                }
            }
        }
        */
        #endregion

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
