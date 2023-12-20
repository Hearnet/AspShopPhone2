using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspShopPhone2.helper
{
    public static class CustomRazorHelpers
    {
        public static string IsActive(int? id, ViewContext viewContext)
        {
            var routeData = viewContext.RouteData;
            var routeIdValue = routeData.Values["id"];
            var routeActionValue = routeData.Values["action"].ToString();
            if (routeIdValue != null && int.TryParse(routeIdValue.ToString(), out int routeId))
            {
                var isActive = id == routeId;
                return isActive ? "category-item--active" : "";
            }

            return "";
        }
    }
}

