using E_Commerce.Web.Models.Dto;
using Microsoft.AspNetCore.SignalR;

namespace E_Commerce.Web.Hubs
{
    public class SesssionManagerHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            ConnectedUser.ConnectionId.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendConnectionId(string connectionId)
        {
            await Clients.All.SendAsync("ReceiveConnectionId", connectionId);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedUser.ConnectionId.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
