﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CJChat.Classes;
using CJChat.Models;
using Microsoft.AspNet.SignalR;

namespace CJChat.Hubs
{
    public class ChatHub : Hub
    {
        public static List<ChatUser> _users = new List<ChatUser>();

        public static EventHandler<MessageReceivedEventArgs> OnMessageReceived;

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

            foreach(var Message in Classes.MessageStore.Messages.OrderBy(x => x.TimeStamp))
            {
                SendMessage(Message.UserName, Message.Message, Message.TimeStamp, ConnectionId);
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
            DateTime TimeStamp = DateTime.UtcNow;

            String SenderIp = Context.Request.GetRemoteIpAddress();

            SendMessage(userName, message, TimeStamp, null);

            SignalMessageReceveived(userName, SenderIp, TimeStamp, message);            
        }

        public void SendMessage(String userName, String message, DateTime timeStamp, String connectionId = null)
        {
            if (String.IsNullOrWhiteSpace(connectionId))
            {
                Clients.All.AddMessage(userName, message, timeStamp.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
            else
            {
                Clients.Client(connectionId).AddMessage(userName, message, timeStamp.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
        }
        #endregion

        #region event raising
        private void SignalMessageReceveived(String userName, String sourceIp, DateTime timeStamp, String message)
        {
            ChatMessage Message = new ChatMessage()
            {
                UserName = userName,
                SoueceIp = sourceIp,
                TimeStamp = timeStamp,
                Message = message
            };

            Interlocked.CompareExchange(ref OnMessageReceived, null, null)?.Invoke(this, new MessageReceivedEventArgs(Message));

        }
        #endregion
    }
}