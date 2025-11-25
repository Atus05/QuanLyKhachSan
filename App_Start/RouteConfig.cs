// File: App_Start/RouteConfig.cs (ĐÃ SỬA)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyKhachSan
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Bỏ qua các file .axd
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // *** DÒNG CẦN THÊM: KÍCH HOẠT ATTRIBUTE ROUTING ***
            routes.MapMvcAttributeRoutes();

            // Route mặc định (chỉ hoạt động khi không có Attribute Route nào trùng khớp)
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "QuanLyKhachSan.Controllers" }
            );
        }
    }
}