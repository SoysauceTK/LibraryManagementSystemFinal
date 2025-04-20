// DEVELOPER: [Both members] - Staff authentication - [Date]
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using LMS.Security;

namespace LibraryManagementSystem.Presentation_Layer.Staff
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Prevent unnecessary redirects and loops
            /*if (User.Identity.IsAuthenticated && !IsPostBack)
            {
                Response.Redirect("~/Presentation Layer/Default.aspx"); // Or redirect to Staff/Dashboard only if safe
            }*/
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (ValidateStaff(Username.Text, Password.Text))
                {
                    FormsAuthentication.SetAuthCookie(Username.Text, false);

                    // Add staff role to the authentication ticket
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(Username.Text, false);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                        ticket.Version,
                        ticket.Name,
                        ticket.IssueDate,
                        ticket.Expiration,
                        ticket.IsPersistent,
                        "Staff", // role data
                        ticket.CookiePath
                    );
                    authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                    Response.Cookies.Add(authCookie);

                    Response.Redirect("~/Presentation Layer/Staff/Dashboard.aspx");
                }
                else
                {
                    FailureText.Text = "Invalid username or password.";
                }
            }
        }

        private bool ValidateStaff(string username, string password)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Staff.xml");
                doc.Load(path);

                XmlNode staffNode = doc.SelectSingleNode($"//staff[username='{username}']");
                if (staffNode != null)
                {
                    string storedHash = staffNode.SelectSingleNode("password").InnerText;
                    return SecurityHelper.VerifyPassword(password, storedHash);
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during login: {ex.Message}");
                FailureText.Text = "An error occurred during login. Please try again.";
                return false;
            }
        }
    }
}
