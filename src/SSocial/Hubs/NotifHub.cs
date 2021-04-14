using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SSocial.Hubs
{
    public class NotifHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public NotifHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }
        
        public async Task OnConnectionReady(string userId)
        {
            var connectionId = Context.ConnectionId;
            Console.WriteLine("Connected: " + userId + " with connectionId " + connectionId);
            _userConnectionManager.AddNewConnection(userId, connectionId);
        }

        //Called when a connection with the hub is terminated.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            Console.WriteLine("Disconnected: " + connectionId);

            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
        
    }
}