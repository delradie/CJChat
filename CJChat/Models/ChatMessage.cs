using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJChat.Models
{
    public class ChatMessage
    {
        public String UserName { get; set; }

        public DateTime TimeStamp { get; set; }

        public String Message { get; set; }

        public String SoueceIp { get; set; }
    }
}