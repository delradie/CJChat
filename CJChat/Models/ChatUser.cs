using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJChat.Models
{
    public class ChatUser
    {
        public String UserName { get; set; }

        public String IPAddress { get; set; }
        public String ConnectionId { get; set; }
    }
}