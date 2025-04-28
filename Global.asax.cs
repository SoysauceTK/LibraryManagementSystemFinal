using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using System.Web.UI; 

namespace LibraryManagementSystem
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            string logPath = Server.MapPath("~/App_Data/startup_log.txt");
            try
            {
                System.IO.File.WriteAllText(logPath, "Application_Start initiated: " + DateTime.Now.ToString() + "\n");

                System.IO.File.AppendAllText(logPath, "Before RegisterRoutes: " + DateTime.Now.ToString() + "\n");
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                System.IO.File.AppendAllText(logPath, "After RegisterRoutes: " + DateTime.Now.ToString() + "\n");

                System.IO.File.AppendAllText(logPath, "Before RegisterBundles: " + DateTime.Now.ToString() + "\n");
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                System.IO.File.AppendAllText(logPath, "After RegisterBundles: " + DateTime.Now.ToString() + "\n");

                // --- BEGIN: Added jQuery Script Resource Mapping ---
                System.IO.File.AppendAllText(logPath, "Before AddDefinition for jquery: " + DateTime.Now.ToString() + "\n");
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
                    new ScriptResourceDefinition
                    {
                        // IMPORTANT: Verify/update the jQuery version number (e.g., 3.4.1) to match your project's file
                        Path = "~/Scripts/jquery-3.7.1.min.js",
                        DebugPath = "~/Scripts/jquery-3.7.1.js",
                        CdnPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.4.1.min.js",
                        CdnDebugPath = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.4.1.js",
                        CdnSupportsSecureConnection = true,
                        LoadSuccessExpression = "window.jQuery"
                    });
                System.IO.File.AppendAllText(logPath, "After AddDefinition for jquery: " + DateTime.Now.ToString() + "\n");
                // --- END: Added jQuery Script Resource Mapping ---

                System.IO.File.AppendAllText(logPath, "Before setting application vars: " + DateTime.Now.ToString() + "\n");
                Application["ActiveUsers"] = 0;
                Application["TotalVisits"] = 0;
                System.IO.File.AppendAllText(logPath, "After setting application vars: " + DateTime.Now.ToString() + "\n");

                System.IO.File.AppendAllText(logPath, "Application_Start completed: " + DateTime.Now.ToString() + "\n");
            }
            catch (Exception ex)
            {
                string errorLogPath = Server.MapPath("~/App_Data/error_log.txt");
                System.IO.File.AppendAllText(errorLogPath, "Error in Application_Start: " + DateTime.Now.ToString() + "\n");
                System.IO.File.AppendAllText(errorLogPath, ex.ToString() + "\n");
                if (ex.InnerException != null)
                {
                    System.IO.File.AppendAllText(errorLogPath, "Inner Exception: " + ex.InnerException.ToString() + "\n");
                }
                // Also log to startup log if possible
                try
                {
                    System.IO.File.AppendAllText(logPath, "FATAL ERROR during Application_Start: " + ex.Message + "\n");
                }
                catch { } // Ignore if startup log itself fails
            }
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            // This method handles role-based authentication
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    string userData = ticket.UserData;

                    // Only create roles array if userData is not empty
                    if (!string.IsNullOrEmpty(userData))
                    {
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                    // If userData is empty, the user remains authenticated but without specific roles attached here.
                    // The RoleProvider will be queried later via User.IsInRole if needed.
                }
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Application.Lock();
            Application["ActiveUsers"] = (int)Application["ActiveUsers"] + 1;
            Application["TotalVisits"] = (int)Application["TotalVisits"] + 1;
            Application.UnLock();

            // Log session start - uncomment if you have log4net configured
            // log4net.ILog logger = log4net.LogManager.GetLogger(GetType());
            // logger.Info($"Session started. Session ID: {Session.SessionID}");
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends
            // Note: This event is reliable only when session state mode is InProc.
            Application.Lock();
            // Ensure ActiveUsers doesn't go below zero if Session_End fires unexpectedly
            if ((int)Application["ActiveUsers"] > 0)
            {
                Application["ActiveUsers"] = (int)Application["ActiveUsers"] - 1;
            }
            Application.UnLock();

            // Log session end - uncomment if you have log4net configured
            // log4net.ILog logger = log4net.LogManager.GetLogger(GetType());
            // logger.Info($"Session ended. Session ID: {Session.SessionID}");
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Get the last error from the server
            Exception ex = Server.GetLastError();

            // Log the error including URL
            try
            {
                string currentUrl = "N/A";
                if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null)
                {
                    currentUrl = HttpContext.Current.Request.Url.ToString();
                }

                string logPath = Server.MapPath("~/App_Data/error_log.txt");
                string errorMessage = $"Error occurred at: {DateTime.Now}\nURL: {currentUrl}\nError: {ex}\n";
                if (ex.InnerException != null)
                {
                    errorMessage += $"Inner Exception: {ex.InnerException}\n";
                }
                errorMessage += "--------------------------------------------------\n";
                System.IO.File.AppendAllText(logPath, errorMessage);
            }
            catch { } // Prevent logging errors from causing further issues

            // Avoid redirect loop if the error happened on the error page itself
            if (Request.Url.AbsolutePath.ToLower().Contains("error.aspx"))
            {
                Server.ClearError(); // Clear the error to prevent yellow screen of death on error page
                return;
            }


            // Redirect to error page for non-AJAX requests
            // Check if headers can be modified (prevents errors if response already sent)
            if (!IsAjaxRequest() && HttpContext.Current.Response.HeadersWritten == false)
            {
                // Clear the error on the server
                Server.ClearError();
                // Optional: Clear any partial response
                // Response.Clear(); // Use with caution, might clear useful info for user
                Response.Redirect("~/Error.aspx", false); // Use false to stop current page execution
                // Ensure the redirect happens immediately
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            // For AJAX requests or if headers are written, let the client handle the error if possible,
            // otherwise the default ASP.NET error page might show or the request might just fail.
            // Consider adding specific AJAX error handling here if needed.
        }

        private bool IsAjaxRequest()
        {
            var request = HttpContext.Current.Request;
            if (request == null) return false;

            // Check standard AJAX header first
            if (request.Headers["X-Requested-With"] == "XMLHttpRequest") return true;

            // Check form variable as a fallback
            if (request.Form["X-Requested-With"] == "XMLHttpRequest") return true;

            // Optional: Check content type for common AJAX patterns
            // if (request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }
    }
}