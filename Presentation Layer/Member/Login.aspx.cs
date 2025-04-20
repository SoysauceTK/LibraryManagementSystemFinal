using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using LMS.Security;

namespace LibraryManagementSystem.Member
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated && !IsPostBack)
            {
                Response.Redirect("~/Presentation Layer/Member/Dashboard.aspx");
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (ValidateUser(Username.Text, Password.Text))
                {
                    FormsAuthentication.RedirectFromLoginPage(Username.Text, RememberMe.Checked);
                }
                else
                {
                    FailureText.Text = "Invalid username or password.";
                }
            }
        }

        private bool ValidateUser(string username, string password)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Member.xml");
                doc.Load(path);

                XmlNode userNode = doc.SelectSingleNode($"//user[username='{username}']");
                if (userNode != null)
                {
                    string storedHash = userNode.SelectSingleNode("password").InnerText;
                    return SecurityHelper.VerifyPassword(password, storedHash);
                }

                return false;
            }
            catch (Exception ex)
            {
                FailureText.Text = "An error occurred during login. Please try again.";
                // Log the exception
                return false;
            }
        }
    }
}