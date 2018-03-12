using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJChat.DataAccess
{
    public class MessageLogAccess
    {
        public IEnumerable<MessageLog> GetRecentMessages(Int32 count)
        {
            if (count <= 0)
            {
                return null;
            }

            using (CJChatEntities Context = new CJChatEntities())
            {
                IEnumerable<MessageLog> Output = Context.MessageLogs
                    .Where(x => !x.Blocked)
                    .OrderByDescending(x => x.UtcTimestamp)
                    .Take(count).ToList();

                return Output;
            }
        }

        public void LogMessage(String userName, String sourceIp, DateTime utcTimestamp, String messageText)
        {
            using (CJChatEntities Context = new CJChatEntities())
            {
                MessageLog NewEntry = Context.MessageLogs.Create();

                NewEntry.UserName = userName;
                NewEntry.SourceIp = sourceIp;
                NewEntry.UtcTimestamp = utcTimestamp;
                NewEntry.MessageText = messageText;
                NewEntry.Blocked = false;

                Context.MessageLogs.Add(NewEntry);

                Context.SaveChanges();
            }
        }

        public void SetBlocked(Int64 messageId, Boolean blocked)
        {
            using (CJChatEntities Context = new CJChatEntities())
            {
                MessageLog UpdateEntry = Context.MessageLogs.Where(x => x.MessageId == messageId).FirstOrDefault();

                if (UpdateEntry == null)
                {
                    return;
                }

                UpdateEntry.Blocked = blocked;

                Context.SaveChanges();
            }
        }
    }
}