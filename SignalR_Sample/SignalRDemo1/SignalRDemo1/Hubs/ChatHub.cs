using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo1.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Login(string name)
        {
            await Clients.AllExcept(Context.ConnectionId).
                SendAsync("online", $"{name} 进入了群聊！");
        }

        public async Task SignOut(string name)
        {
            await Clients.AllExcept(Context.ConnectionId)
                .SendAsync("online", $"{name} 离开了群聊！");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageByServer(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, "系统通知:" + message);
        }
    }
}
