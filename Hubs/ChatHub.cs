using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloFuture.Hubs
{

        public class ChatHub : Hub
        {
            public async Task SendMessage(string CurrentUser, string SenderId, string RecieverId, string message)
            {
                var OtherUser = (CurrentUser == SenderId) ? RecieverId : SenderId;
                //await Clients.All.SendAsync("ReceiveMessage", CurrentUser, SenderId, RecieverId, message);
                await Clients.User(OtherUser).SendAsync("ReceiveMessage", CurrentUser, SenderId, RecieverId, message);
            }
        }
    
}
