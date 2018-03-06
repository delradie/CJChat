using CJChat.Models;
using System;

namespace CJChat.Classes
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public ChatMessage ReceivedMessage { get; }

        public MessageReceivedEventArgs(ChatMessage receivedMessage) : base()
        {
            this.ReceivedMessage = receivedMessage;
        }
    }
}