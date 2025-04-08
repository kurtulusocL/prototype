using Core.Entities.EntityFramwoek;
using Entities.Entities.User;

namespace Entities.Entities
{
    public class Audit : BaseEntity
    {
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string AreaAccessed { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        public string DeviceModel { get; set; }
        public string Language { get; set; }
        public string Platform { get; set; }
        public string MacAddress { get; set; }
        public string LocalIPAddress { get; set; }
        public string RemoteIpAddress { get; set; }
        public string IpAddressVPN { get; set; }
        public string Host { get; set; }
        public string ProxyConnection { get; set; }
        public string Manufacturer { get; set; }
    }
}
