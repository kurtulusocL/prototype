using System.Security.Claims;
using System.Text.Json;
using Core.Audit;
using Core.Helper;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Attibutes
{
    public class AuditLogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var scope = ServiceProviderHelper.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();               
                var request = filterContext.HttpContext.Request;
                var userAgent = request.HttpContext.Request.Headers["User-Agent"].ToString();
                string macAddress = DeviceManufacturer.GetMACAddress();
                Audit audit = new Audit()
                {
                    Username = (request.HttpContext.User.Identity.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous",
                    IPAddress = IpAddress.FindUserIp(),
                    Browser = request.HttpContext.Request.Headers["User-Agent"].ToString(),
                    Language = request.HttpContext.Request.Headers["Accept-Language"].ToString(),
                    AreaAccessed = AreaAccessed.GetAreaAccessed(filterContext),
                    Host = request.HttpContext.Request.Host.ToString(),
                    ProxyConnection = request.HttpContext.Request.Headers["Connection"],
                    Device = DeviceType.GetDeviceType(userAgent),
                    DeviceModel = DeviceModel.GetDeviceModel(userAgent),
                    Platform = Platform.GetPlatform(userAgent),
                    Manufacturer = DeviceManufacturer.GetManufacturerFromMac(macAddress),
                    MacAddress = MacAddress.GetMacAddress(),
                    LocalIPAddress = LocalIpAddress.GetLocalIPAddress(),
                    RemoteIpAddress = RemoteIpAddress.GetRemoteIpAddress(request.HttpContext),
                    IpAddressVPN = IpAddressWithVpn.GetClientIPAddress(request.HttpContext),
                    CreatedDate = DateTime.Now
                };

                dbContext.Audits.Add(audit);
                SaveToFileAsJson(audit);
                dbContext.SaveChanges();
                base.OnActionExecuting(filterContext);
            }
        }
        private void SaveToFileAsJson(Audit audit)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AuditLogs.txt");
            var jsonString = JsonSerializer.Serialize(audit, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.AppendAllText(filePath, jsonString + Environment.NewLine);
        }
    }
}