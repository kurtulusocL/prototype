using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Audit
{
    public static class DeviceType
    {
        public static string GetDeviceType(string userAgent)
        {
            if (userAgent.Contains("Mobile")) return "Mobile";
            if (userAgent.Contains("Tablet")) return "Tablet";
            return "Desktop";
        }
    }
}
