// Global.asax.cs
// DEVELOPER: [Both members] - Global event handlers - [Date]
using System;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI; // Required for ScriptManager & ScriptResourceDefinition

namespace LibraryManagementSystem
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Register routes
            RegisterRoutes(RouteTable.Routes);

            // Register jQuery for unobtrusive validation
            ScriptResourceDefinition jquery = new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.6.0.min.js",
                DebugPath = "~/Scripts/jquery-3.6.0.js",
                CdnPath = "https://code.jquery.com/jquery-3.6.0.min.js",
                CdnDebugPath = "https://code.jquery.com/jquery-3.6.0.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            };
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", jquery);

            // Application-wide settings
            Application["ActiveUsers"] = 0;
            Application["TotalVisits"] = 0;

            // Logging
            log4net.Config.XmlConfigurator.Configure();
        }

        void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] + 1;
            Application["TotalVisits"] = (int)Application["TotalVisits"] + 1;
            Application.UnLock();

            log4net.ILog logger = log4net.LogManager.GetLogger(GetType());
            logger.Info($"Session started. Session ID: {Session.SessionID}");
        }

        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] - 1;
            Application.UnLock();

            log4net.ILog logger = log4net.LogManager.GetLogger(GetType());
            logger.Info($"Session ended. Session ID: {Session.SessionID}");
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            log4net.ILog logger = log4net.LogManager.GetLogger(GetType());
            logger.Error("An unhandled exception occurred", ex);

            // Optional: redirect to error page if needed
            // string currentPath = HttpContext.Current?.Request?.Path.ToLower() ?? "";
            // if (!IsAjaxRequest() && !currentPath.Contains("/error.aspx"))
            // {
            //     Response.Clear();
            //     Server.ClearError();
            //     Response.Redirect("~/Presentation Layer/Error.aspx");
            // }
        }

        private bool IsAjaxRequest()
        {
            var request = HttpContext.Current.Request;
            return request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                   request.Form["X-Requested-With"] == "XMLHttpRequest";
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");
            routes.MapPageRoute(
                "Default",
                "",
                "~/Presentation Layer/Public/Default.aspx");
        }
    }
}
