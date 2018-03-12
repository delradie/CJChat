using CJChat.DataAccess;
using CJChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJChat.Classes
{
    public static class MessageStore
    {
        private const Int32 HistoryLimit = 50;

        public static List<ChatMessage> Messages { get; private set; }

        static MessageStore()
        {
            Messages = new List<ChatMessage>();
        }

        public static void Init()
        {
            IEnumerable<MessageLog> HistoricalMessages = new MessageLogAccess().GetRecentMessages(HistoryLimit);

            if(HistoricalMessages != null && HistoricalMessages.Count() > 0)
            {
                Messages.AddRange(
                    HistoricalMessages.Select(
                        
                        x => new ChatMessage()
                        {
                            Message = x.MessageText,
                            SoueceIp = x.SourceIp,
                            TimeStamp = x.UtcTimestamp,
                            UserName = x.UserName
                        })
                    );
            }
        }

        public static void MessageReceivedHandler(Object sender, MessageReceivedEventArgs args)
        {
            Messages.Add(args.ReceivedMessage);

            if(Messages.Count > HistoryLimit)
            {
                Messages = Messages.OrderByDescending(x => x.TimeStamp).Take(HistoryLimit).ToList();
            }

            new MessageLogAccess().LogMessage(args.ReceivedMessage.UserName, args.ReceivedMessage.SoueceIp, args.ReceivedMessage.TimeStamp, args.ReceivedMessage.Message);
        }
    }
}