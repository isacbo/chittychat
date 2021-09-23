using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chittychat.Services
{
    public class UserTracker
    {
        // Connections is a dictionary of connections mapping to a user name
        Dictionary<string, string> connections;
        // users is a dictionary of usernames mapping to some user data. TODO: replace bool later.
        Dictionary<string, bool> users;

        public string[] Users
        {
            get { return users.Keys.ToArray(); }
        }

        public UserTracker()
        {
            connections = new Dictionary<string, string>();
            users = new Dictionary<string, bool>();
        }

        public string Doggy
        {
            get { return $"Soggy Doggy {connections.Keys.Count}"; }
        }

        /// <summary>
        /// Returns true if the connection is the first
        /// connection of the user.
        /// </summary>
        /// <param name="connectionId">connection id from signalr</param>
        /// <param name="user">Unique username</param>
        /// <returns></returns>
        public bool AddConnection(string connectionId, string user)
        {
            bool ret = false;
            if (!users.ContainsKey(user))
            {
                ret = true;
                users.Add(user, true);
            }

            connections.Add(connectionId, user);

            return ret;
        }

        public string[] GetUserConnections(string username)
        {
            // TODO: Can be optimized A LOT.
            List<string> cons = new List<string>();
            foreach (string con in connections.Keys)
            {
                if (connections[con] == username)
                {
                    cons.Add(con);
                }
            }

            return cons.ToArray();
        }

        /// <summary>
        /// Returns true if the user has no more connections after removing this connection.
        /// </summary>
        /// <param name="connectionId">connection id from signalr</param>
        /// <returns></returns>
        public bool RemoveConnection(string connectionId)
        {
            if (!connections.ContainsKey(connectionId)) return false;
            string user = connections[connectionId];
            connections.Remove(connectionId);

            if ( !connections.ContainsValue(user) )
            {
                users.Remove(user);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
