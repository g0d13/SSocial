using System;
using System.Collections.Generic;

namespace SSocial.Hubs
{
    public interface IUserConnectionManager
    {
        void AddNewConnection(string userId, string connectionId);
        void RemoveUserConnection(string connectionId);

        List<string> GetUserConnections(string userId);
        
        List<string> GetOnlineUsers();
    }
}