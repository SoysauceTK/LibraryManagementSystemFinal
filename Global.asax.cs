using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using log4net;

namespace LibraryManagementSystem
{
    public class Global : HttpApplication
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Global));

        void Application_Start(object sender, EventArgs e)
        {
            // Register routes
            RegisterRoutes(RouteTable.Routes);

            // Register jQuery
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

            // Configure log4net
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Skip authentication check for TryIt pages and CAPTCHA handler
            if (Request.Path.Contains("TryIt.aspx") ||
                Request.Path.Contains("CaptchaImage.ashx"))
            {
                return;
            }

            if (Request.IsAuthenticated && !IsAjaxRequest())
            {
                try
                {
                    // Skip if already on a dashboard page, error page, or login page
                    string currentPath = Request.Path.ToLower();
                    if (currentPath.Contains("dashboard.aspx") ||
                        currentPath.Contains("error.aspx") ||
                        currentPath.Contains("login.aspx") ||
                        currentPath.Contains("register.aspx"))
                    {
                        return;
                    }

                    // Skip if this is a CAPTCHA refresh request
                    if (Request.Form.AllKeys.Contains("__EVENTTARGET") &&
                        Request.Form["__EVENTTARGET"].Contains("btnRefresh"))
                    {
                        return;
                    }

                    string username = Context.User.Identity.Name;

                    // Role-based redirect logic
                    if (Roles.IsUserInRole(username, "Staff"))
                    {
                        if (!currentPath.Contains("staff/dashboard.aspx"))
                        {
                            Response.Redirect("~/Presentation Layer/Staff/Dashboard.aspx");
                        }
                    }
                    else if (Roles.IsUserInRole(username, "Member"))
                    {
                        if (!currentPath.Contains("member/dashboard.aspx"))
                        {
                            Response.Redirect("~/Presentation Layer/Member/Dashboard.aspx");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error in Application_AuthenticateRequest", ex);
                }
            }
        }
        void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] + 1;
            Application["TotalVisits"] = (int)Application["TotalVisits"] + 1;
            Application.UnLock();

            logger.Info($"Session started. Session ID: {Session.SessionID}");
        }

        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] - 1;
            Application.UnLock();

            logger.Info($"Session ended. Session ID: {Session.SessionID}");
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            logger.Error("An unhandled exception occurred", ex);

            // Handle specific HTTP errors
            HttpException httpEx = ex as HttpException;
            if (httpEx != null && httpEx.GetHttpCode() == 404)
            {
                Response.Redirect("~/Error.aspx?code=404");
            }
            else
            {
                Response.Redirect("~/Error.aspx");
            }
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

            // Public routes
            routes.MapPageRoute(
                "Default",
                "",
                "~/Presentation Layer/Public/Default.aspx");

            // Member routes
            routes.MapPageRoute(
                "MemberLogin",
                "member/login",
                "~/Presentation Layer/Member/Login.aspx");
            routes.MapPageRoute(
                "MemberDashboard",
                "member/dashboard",
                "~/Presentation Layer/Member/Dashboard.aspx");

            // Staff routes
            routes.MapPageRoute(
                "StaffLogin",
                "staff/login",
                "~/Presentation Layer/Staff/Login.aspx");
            routes.MapPageRoute(
                "StaffDashboard",
                "staff/dashboard",
                "~/Presentation Layer/Staff/Dashboard.aspx");

            // TryIt routes
            routes.MapPageRoute(
                "BookStorageTryIt",
                "services/bookstorage/tryit",
                "~/Service Layer/Book Storage Service/BookStorageTryIt.aspx");
            routes.MapPageRoute(
                "SearchTryIt",
                "services/search/tryit",
                "~/Service Layer/Book Search Service/SearchTryIt.aspx");
        }
    }
}