using CJChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJChat.Classes
{
    public static class MessageStore
    {
        private const Int32 HistoryLimit = 5;

        public static List<ChatMessage> Messages { get; private set; }

        static MessageStore()
        {
            Messages = new List<ChatMessage>();
        }

        public static void MessageReceivedHandler(Object sender, MessageReceivedEventArgs args)
        {
            Messages.Add(args.ReceivedMessage);

            if(Messages.Count > HistoryLimit)
            {
                Messages = Messages.OrderByDescending(x => x.TimeStamp).Take(HistoryLimit).ToList();
            }
        }
    }
}