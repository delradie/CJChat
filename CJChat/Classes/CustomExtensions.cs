using Microsoft.AspNet.SignalR;
using System.Web;

namespace CJChat.Classes
{
    public static class CustomExtensions
    {
        public static HttpContextBase GetHttpContext(this IRequest request)
        {
            object httpContextBaseValue;
            if (request.Environment.TryGetValue(typeof(HttpContextBase).FullName, out httpContextBaseValue))
            {
                return httpContextBaseValue as HttpContextBase;
            }
            return null;
        }
        public static string GetRemoteIpAddress(this IRequest request)
        {
            object ipAddress;
            //server.LocalIpAddress
            if (request.Environment.TryGetValue("server.RemoteIpAddress", out ipAddress))
            {
                return ipAddress as string;
            }
            return "";
        }
    }
}