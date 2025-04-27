using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using LMS.Security;

namespace LibraryManagementSystem.Member
{
    // DEVELOPER: Aarya Baireddy - Member authentication
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect if user is already logged in
            if (User.Identity.IsAuthenticated)
            {
                // If they're already logged in, sign them out first
                if (Request.QueryString["ReturnUrl"] != null &&
                    Request.QueryString["ReturnUrl"].ToLower().Contains("staff"))
                {
                    // They're trying to access Staff area as a Member, sign out
                    FormsAuthentication.SignOut();
                    Response.Clear();
                }
                else
                {
                    // Regular Member login, redirect to dashboard
                    Response.Redirect("~/Member/Dashboard.aspx");
                }
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate against Member.xml
                if (ValidateUser(Username.Text, Password.Text))
                {
                    // Create authentication cookie
                    FormsAuthentication.SetAuthCookie(Username.Text, RememberMe.Checked);

                    // Add Member role
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(Username.Text, RememberMe.Checked);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                        ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                        ticket.IsPersistent, "Member", ticket.CookiePath);
                    authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                    Response.Cookies.Add(authCookie);

                    // Redirect to requested URL or default page
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl) && !returnUrl.ToLower().Contains("staff") && IsLocalUrl(returnUrl))
                    {
                        Response.Redirect(returnUrl);
                    }
                    else
                    {
                        Response.Redirect("~/Member/Dashboard.aspx");
                    }
                }
                else
                {
                    FailureText.Text = "<div class='alert alert-danger'>Invalid username or password.</div>";
                }
            }
        }

        private bool ValidateUser(string username, string password)
        {
            try
            {
                // Load Member.xml
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Member.xml");
                doc.Load(path);

                // Find user node
                XmlNode userNode = doc.SelectSingleNode($"//user[username='{username}']");
                if (userNode != null)
                {
                    // Get stored password hash
                    string storedHash = userNode.SelectSingleNode("password").InnerText;

                    // Use SecurityHelper to verify the password
                    return SecurityHelper.VerifyPassword(password, storedHash);
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error
                string logPath = Server.MapPath("~/App_Data/error_log.txt");
                System.IO.File.AppendAllText(logPath, "Error in Member Login: " + DateTime.Now.ToString() + "\n");
                System.IO.File.AppendAllText(logPath, ex.ToString() + "\n");

                FailureText.Text = "<div class='alert alert-danger'>An error occurred during login. Please try again.</div>";
                return false;
            }
        }

        private bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            Uri absoluteUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                return String.Equals(absoluteUri.Host, Request.Url.Host,
                    StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
                    && !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
                    && Uri.IsWellFormedUriString(url, UriKind.Relative);
                return isLocal;
            }
        }
    }
}