using System;
using System.Web.Routing;

namespace LibraryManagementSystem
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();

            routes.MapPageRoute(
                "Default",
                "",
                "~/Default.aspx"
            );

            routes.MapPageRoute(
                "Member-Login",
                "Member/Login",
                "~/Member/Login.aspx"
            );

            routes.MapPageRoute(
                "Member-Register",
                "Member/Register",
                "~/Member/Register.aspx"
            );

            routes.MapPageRoute(
                "Member-Dashboard",
                "Member/Dashboard",
                "~/Member/Dashboard.aspx"
            );

            routes.MapPageRoute(
                "Staff-Login",
                "Staff/Login",
                "~/Staff/Login.aspx"
            );

            routes.MapPageRoute(
                "Staff-Dashboard",
                "Staff/Dashboard",
                "~/Staff/Dashboard.aspx"
            );
        }
    }
}