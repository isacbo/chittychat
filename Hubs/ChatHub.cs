using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using chittychat.Services;

namespace chittychat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        // Service provided through dependency injection.
        private UserTracker m_userTracker;

        public ChatHub(UserTracker userTracker)
        {
            m_userTracker = userTracker;
        }

        // Some wrappers because I am lazy.
        public string Username
        {
            get { return Context.User.Identity.Name; }
        }

        public string Id
        {
            get { return Context.ConnectionId; }
        }

        public override Task OnConnectedAsync()
        {
            if (m_userTracker.AddConnection(Id, Username))
            {
                Console.WriteLine($"{Username} has connected.");
                Clients.AllExcept(Context.ConnectionId).SendAsync("UserLogIn", Username);
            }
            Clients.Caller.SendAsync("OnConnect", m_userTracker.Users);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (m_userTracker.RemoveConnection(Id))
            {
                Console.WriteLine($"{Username} has disconnected.");
                Clients.All.SendAsync("UserLogOut", Username);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            var claims = Context.User.Claims;
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message);
        }

        public async Task SendDirectMessage(string toUser, string message)
        {
            string[] cons = m_userTracker.GetUserConnections(toUser);
            // TODO: make this into one call.
            await Clients.Caller.SendAsync("ReceiveDirectMessage", Context.User.Identity.Name, message);
            await Clients.Clients(cons).SendAsync("ReceiveDirectMessage", Context.User.Identity.Name, message);
        }

    }
}