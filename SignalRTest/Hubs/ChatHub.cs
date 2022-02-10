using Microsoft.AspNetCore.SignalR;

namespace SignalRTest.Hubs
{
    public class ChatHub : Hub
    {
        static int count = 0;
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendMessageToCaller(string user, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendMessageToUser(string connectionId, string user, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            count = count + 1;
            //Clients.All.UpdateCount(count);
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId, "Mohammed");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}
