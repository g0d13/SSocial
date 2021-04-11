using System;
using System.Collections.Generic;
using System.Linq;

namespace SSocial.Hubs
{
    public class UserConnectionManager : IUserConnectionManager
    {
        private static readonly Dictionary<string, List<string>> UserConnectionMap = new();
        
        public void KeepUserConnection(string userId, string connectionId)
        {
            lock (UserConnectionMap)
            {
                if (!UserConnectionMap.ContainsKey(userId))
                {
                    UserConnectionMap[userId] = new List<string>();
                }
                UserConnectionMap[userId].Add(connectionId);
            }
        }

        public void RemoveUserConnection(string connectionId)
        {
            lock (UserConnectionMap)
            {
                foreach (var userId in UserConnectionMap.Keys
                    .Where(userId => UserConnectionMap.ContainsKey(userId))
                    .Where(userId => UserConnectionMap[userId].Contains(connectionId)))
                {
                    UserConnectionMap[userId].Remove(connectionId);
                    break;
                }
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            List<string> conn;
            lock (UserConnectionMap)
            {
                conn = UserConnectionMap[userId];
            }
            return conn;
        }

        public List<string> GetOnlineUsers()
        {
            List<string> connected;
            lock (UserConnectionMap)
            {
                var keys = UserConnectionMap.Keys.ToList();
                connected = keys;
            }

            return connected;
        }
    }
}