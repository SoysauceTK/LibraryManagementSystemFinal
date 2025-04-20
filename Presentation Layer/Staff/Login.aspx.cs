using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using LMS.Security;
using LibraryManagementSystem.Local_Component_Layer.Controls;

namespace LibraryManagementSystem.Presentation_Layer.Staff
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated && !IsPostBack)
            {
                Response.Redirect("~/Presentation Layer/Staff/Dashboard.aspx");
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (!StaffCaptcha.Validate())
            {
                FailureText.Text = "Invalid CAPTCHA code. Please try again.";
                return;
            }

            if (IsValid && ValidateUser(Username.Text, Password.Text))
            {
                FormsAuthentication.SetAuthCookie(Username.Text, RememberMe.Checked);

                if (Roles.IsUserInRole(Username.Text, "Staff"))
                {
                    Response.Redirect("~/Presentation Layer/Staff/Dashboard.aspx");
                }
                else
                {
                    FormsAuthentication.SignOut();
                    FailureText.Text = "Invalid staff credentials.";
                }
            }
            else
            {
                FailureText.Text = "Invalid username or password.";
            }
        }

        private bool ValidateUser(string username, string password)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Staff.xml");
                doc.Load(path);

                XmlNode userNode = doc.SelectSingleNode($"//staff[username='{username}']");
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
                return false;
            }
        }
    }
}