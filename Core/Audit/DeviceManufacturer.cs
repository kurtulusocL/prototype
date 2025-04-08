using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Core.Audit
{
    public static class DeviceManufacturer
    {
        private static readonly Dictionary<string, string> OuiLookup = new Dictionary<string, string>
        {
            { "00:14:22", "Dell" },
            { "00:50:56", "VMware" },
            { "00:1A:11", "Lenovo" },
            { "00:25:00", "Apple" },
            { "00:A0:60", "Acer"},
            { "08:BF:B8", "Asus"},
            { "E8:B0:C5", "Monster"},
            { "EA:B0:C5", "Monster"}
            // For More (IEEE list: https://standards-oui.ieee.org/)
        };
        public static string GetMACAddress()
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
            catch (Exception)
            {
                return "Unknown MAC Address";
            }
            return "Unknown MAC Address";
        }
        public static string GetManufacturerFromMac(string macAddress)
        {
            if (string.IsNullOrEmpty(macAddress) || macAddress.Length < 8)
            {
                return "Unknown Manufacturer";
            }
            macAddress = macAddress.Replace("-", ":").ToUpper();
            if (macAddress.Length == 12 && !macAddress.Contains(":"))
            {
                macAddress = string.Join(":", Enumerable.Range(0, macAddress.Length / 2)
                    .Select(i => macAddress.Substring(i * 2, 2)));
            }
            var oui = macAddress.Substring(0, 8);
            return OuiLookup.TryGetValue(oui, out var manufacturer) ? manufacturer : "Unknown Manufacturer";
        }
    }
}
