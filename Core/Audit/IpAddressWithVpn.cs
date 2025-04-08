using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Audit
{
    public static class IpAddressWithVpn
    {
        public static string GetClientIPAddress(HttpContext context)
        {
            var forwardedHeader = context.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                var ip = forwardedHeader.Split(',')[0].Trim();
                return ip;
            }

            var forwarded = context.Request.Headers["Forwarded"].ToString();
            if (!string.IsNullOrEmpty(forwarded))
            {
                var forwardedIp = forwarded.Split(';').Select(x => x.Trim()).FirstOrDefault(x => x.StartsWith("for="))?.Replace("for=", "").Trim();
                if (!string.IsNullOrEmpty(forwardedIp))
                {
                    return forwardedIp;
                }
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
