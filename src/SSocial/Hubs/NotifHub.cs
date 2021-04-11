using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SSocial.Hubs
{
    [Authorize]
    public class NotifHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public NotifHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }

        public override Task OnConnectedAsync()
        {
            var identity = Context.User?.FindFirst(ClaimTypes.Actor)?.Value;
            var connectionId = Context.ConnectionId;
            _userConnectionManager.KeepUserConnection(identity,connectionId );
            _userConnectionManager.GetOnlineUsers();
            return base.OnConnectedAsync();
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        
        //Called when a connection with the hub is terminated.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}