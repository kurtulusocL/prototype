using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Audit
{
    public static class AreaAccessed
    {
        public static string GetAreaAccessed(ActionExecutingContext filterContext)
        {
            var routeData = filterContext.RouteData;
            var controllerName = routeData.Values["controller"]?.ToString();
            var actionName = routeData.Values["action"]?.ToString();

            var areaName = routeData.DataTokens["area"]?.ToString();

            if (!string.IsNullOrEmpty(areaName))
            {
                return $"{areaName}/{controllerName}/{actionName}";
            }
            return $"{controllerName}/{actionName}";
        }
    }
}
