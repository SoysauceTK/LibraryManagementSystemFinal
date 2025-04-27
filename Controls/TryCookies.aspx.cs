using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryManagementSystem.Controls
{
    public partial class TryCookies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set default values
                txtCookieName.Text = "TestCookie";
                txtCookieValue.Text = "Hello, World!";
                txtSessionKey.Text = "TestSessionVar";
                txtSessionValue.Text = "Session Value";
                
                // Store session start time if not already set
                if (Session["SessionStartTime"] == null)
                {
                    Session["SessionStartTime"] = DateTime.Now;
                }
            }

            // Always refresh the displays
            DisplayCookies();
            DisplaySessionInfo();
            DisplayApplicationStats();
        }

        #region Cookie Management

        protected void btnSetCookie_Click(object sender, EventArgs e)
        {
            string cookieName = txtCookieName.Text.Trim();
            string cookieValue = txtCookieValue.Text.Trim();
            
            if (string.IsNullOrEmpty(cookieName))
            {
                DisplayErrorMessage("Cookie name cannot be empty.");
                return;
            }

            try
            {
                HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
                
                // Set expiration if not a session cookie
                int expirationMinutes = int.Parse(ddlExpiration.SelectedValue);
                if (expirationMinutes > 0)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(expirationMinutes);
                }
                
                // Add or update the cookie
                Response.Cookies.Add(cookie);
                
                DisplaySuccessMessage($"Cookie '{cookieName}' has been set successfully.");
                DisplayCookies();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage($"Error setting cookie: {ex.Message}");
            }
        }

        protected void btnDeleteCookie_Click(object sender, EventArgs e)
        {
            string cookieName = txtCookieName.Text.Trim();
            
            if (string.IsNullOrEmpty(cookieName))
            {
                DisplayErrorMessage("Cookie name cannot be empty.");
                return;
            }

            try
            {
                if (Request.Cookies[cookieName] != null)
                {
                    HttpCookie cookie = new HttpCookie(cookieName);
                    cookie.Expires = DateTime.Now.AddDays(-1); // Set expiration in the past
                    Response.Cookies.Add(cookie); // Overwrite the cookie with the expired one
                    
                    DisplaySuccessMessage($"Cookie '{cookieName}' has been deleted successfully.");
                }
                else
                {
                    DisplayWarningMessage($"Cookie '{cookieName}' does not exist.");
                }
                
                DisplayCookies();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage($"Error deleting cookie: {ex.Message}");
            }
        }

        private void DisplayCookies()
        {
            StringBuilder cookiesHtml = new StringBuilder();
            
            if (Request.Cookies.Count > 0)
            {
                cookiesHtml.Append("<table class='table table-striped'>");
                cookiesHtml.Append("<thead><tr><th>Name</th><th>Value</th><th>Expires</th></tr></thead>");
                cookiesHtml.Append("<tbody>");
                
                foreach (string cookieName in Request.Cookies.AllKeys)
                {
                    HttpCookie cookie = Request.Cookies[cookieName];
                    cookiesHtml.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                        HttpUtility.HtmlEncode(cookie.Name),
                        HttpUtility.HtmlEncode(cookie.Value),
                        cookie.Expires != DateTime.MinValue ? cookie.Expires.ToString() : "Session Cookie"
                    );
                }
                
                cookiesHtml.Append("</tbody></table>");
            }
            else
            {
                cookiesHtml.Append("<div class='alert alert-warning'>No cookies found.</div>");
            }
            
            litCookies.Text = cookiesHtml.ToString();
        }

        #endregion

        #region Session Management

        protected void btnSetSession_Click(object sender, EventArgs e)
        {
            string key = txtSessionKey.Text.Trim();
            string value = txtSessionValue.Text.Trim();
            
            if (string.IsNullOrEmpty(key))
            {
                DisplayErrorMessage("Session variable name cannot be empty.");
                return;
            }

            try
            {
                // Set the session variable
                Session[key] = value;
                
                DisplaySuccessMessage($"Session variable '{key}' has been set successfully.");
                DisplaySessionInfo();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage($"Error setting session variable: {ex.Message}");
            }
        }

        protected void btnClearSession_Click(object sender, EventArgs e)
        {
            try
            {
                // Save the session start time
                DateTime? sessionStartTime = Session["SessionStartTime"] as DateTime?;
                
                // Clear all session variables
                Session.Clear();
                
                // Restore the session start time
                if (sessionStartTime.HasValue)
                {
                    Session["SessionStartTime"] = sessionStartTime.Value;
                }
                else
                {
                    Session["SessionStartTime"] = DateTime.Now;
                }
                
                DisplaySuccessMessage("All session variables have been cleared.");
                DisplaySessionInfo();
            }
            catch (Exception ex)
            {
                DisplayErrorMessage($"Error clearing session: {ex.Message}");
            }
        }

        private void DisplaySessionInfo()
        {
            // Display session ID
            litSessionId.Text = Session.SessionID;
            
            // Display session creation time
            if (Session["SessionStartTime"] != null)
            {
                DateTime startTime = (DateTime)Session["SessionStartTime"];
                litSessionStart.Text = startTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                litSessionStart.Text = "Unknown";
            }
            
            // Display session timeout
            litSessionTimeout.Text = Session.Timeout.ToString();
            
            // Display session variables
            DisplaySessionVariables();
        }

        private void DisplaySessionVariables()
        {
            StringBuilder sessionVarsHtml = new StringBuilder();
            
            if (Session.Count > 0)
            {
                sessionVarsHtml.Append("<table class='table table-striped'>");
                sessionVarsHtml.Append("<thead><tr><th>Name</th><th>Value</th><th>Type</th></tr></thead>");
                sessionVarsHtml.Append("<tbody>");
                
                foreach (string key in Session.Keys)
                {
                    object value = Session[key];
                    string displayValue = (value != null) ? HttpUtility.HtmlEncode(value.ToString()) : "null";
                    string typeName = (value != null) ? value.GetType().Name : "null";
                    
                    sessionVarsHtml.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                        HttpUtility.HtmlEncode(key),
                        displayValue,
                        typeName
                    );
                }
                
                sessionVarsHtml.Append("</tbody></table>");
            }
            else
            {
                sessionVarsHtml.Append("<div class='alert alert-warning'>No session variables found.</div>");
            }
            
            litSessionVars.Text = sessionVarsHtml.ToString();
        }

        #endregion

        #region Application State

        private void DisplayApplicationStats()
        {
            // Display application state variables
            litActiveUsers.Text = Application["ActiveUsers"]?.ToString() ?? "0";
            litTotalVisits.Text = Application["TotalVisits"]?.ToString() ?? "0";
        }

        #endregion

        #region Utility Methods

        private void DisplaySuccessMessage(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", 
                $"alert('{message}');", true);
        }

        private void DisplayWarningMessage(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "WarningMessage", 
                $"alert('{message}');", true);
        }

        private void DisplayErrorMessage(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", 
                $"alert('Error: {message}');", true);
        }

        #endregion
    }
} 