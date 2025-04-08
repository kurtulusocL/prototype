using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Audit
{
    public static class RemoteIpAddress
    {
        public static string GetRemoteIpAddress(HttpContext context)
        {
            var forwardedHeader = context.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                return forwardedHeader.Split(',')[0].Trim();
            }

            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp == null)
            {
                return "Unknown";
            }

            if (remoteIp.IsIPv4MappedToIPv6)
            {
                return remoteIp.MapToIPv4().ToString();
            }
            return remoteIp.ToString();
        }
    }
}
