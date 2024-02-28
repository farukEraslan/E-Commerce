using E_Commerce.Web.Models.Dto;
using E_Commerce.Web.Models.Dto.Request;
using E_Commerce.Web.Models.Dto.Response;
using E_Commerce.Web.Services.IServices;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
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
        /// SignalR'a bir istemci bağlandığında client'in connectionId'sini static bir listeye ekler.
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            //ConnectedUser.ConnectionId.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// SignalR'dan bir istemci bağlantısını kopardığında client'in connectionId'sini static bir listeden çıkarır.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            //ConnectedUser.ConnectionId.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendConnectionId(string connectionId)
        {
            await Clients.All.SendAsync("ReceiveConnectionId", connectionId);
        }

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
                    //await Clients.Caller.SendAsync();
                }
                ConnectedUser.ConnectionId.Add(key: Context.ConnectionId, value: userId);
                var test = ConnectedUser.ConnectionId;
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
