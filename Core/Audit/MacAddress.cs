using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Core.Audit
{
    public static class MacAddress
    {
        public static string GetMacAddress()
        {
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up &&
                        adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                        adapter.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        string macAddress = adapter.GetPhysicalAddress().ToString();
                        if (!string.IsNullOrEmpty(macAddress))
                        {
                            return string.Join(":", Enumerable.Range(0, macAddress.Length / 2)
                                .Select(i => macAddress.Substring(i * 2, 2)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the entity.", ex.InnerException);
            }
            return "Unknown MAC Address";
        }
    }
}
