using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CJChat.Models;
using Microsoft.AspNet.SignalR;

namespace CJChat.Hubs
{
    public class ChatHub : Hub
    {
        public static List<ChatUser> _users = new List<ChatUser>();

        #region connection handlers

        public override Task OnDisconnected(Boolean stopCalled)
        {
            RemoveUser();
            UpdateUsers();

            return base.OnDisconnected(stopCalled);
        }
        #endregion

        #region user management
        public void AddUser(String userName)
        {
            String ConnectionId = Context.ConnectionId;

            if (String.IsNullOrWhiteSpace(userName))
            {
                return;
            }

            String SenderIp = Context.Request.GetRemoteIpAddress();

            ChatUser Target = _users.FirstOrDefault(x => String.Equals(x.UserName, userName, StringComparison.InvariantCultureIgnoreCase));

            if (Target == null)
            {
                _users.Add(new ChatUser()
                {
                    UserName = userName,
                    IPAddress = SenderIp,
                    ConnectionId = ConnectionId
                });
            }
            else
            {
                Target.ConnectionId = ConnectionId;
            }

            UpdateUsers();
        }

        private void RemoveUser()
        {
            String ConnectionId = Context.ConnectionId;

            ChatUser TargetUser = _users.FirstOrDefault(x => x.ConnectionId == ConnectionId);

            if (TargetUser != null)
            {
                _users.Remove(TargetUser);
            }
        }
        #endregion

        #region Javascript interaction
        public void UpdateUsers()
        {
            var UserData = _users.Select(x => x.UserName);

            Clients.All.UpdateUsers(UserData);
        }

        public void Send(String userName, String message)
        {
            Clients.All.AddMessage(userName, message);
        }
        #endregion
    }
}