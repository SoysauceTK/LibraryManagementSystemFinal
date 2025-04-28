using System;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using LMS.Security;

namespace LibraryManagementSystem.Member
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Member/Dashboard.aspx");
            }
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Verify CAPTCHA
                if (!RegisterCaptcha.Validate())
                {
                    StatusMessage.Text = "<div class='alert alert-danger'>Invalid verification code. Please try again.</div>";
                    RegisterCaptcha.GenerateCaptcha();
                    return;
                }

                // Check if the username already exists
                if (UserExists(Username.Text))
                {
                    StatusMessage.Text = "<div class='alert alert-danger'>Username already exists. Please choose a different username.</div>";
                    return;
                }

                // Add the user to Member.xml
                if (AddUser(Username.Text, Password.Text, Email.Text))
                {
                    // Log in the new user
                    FormsAuthentication.SetAuthCookie(Username.Text, false);

                    // Redirect to dashboard
                    Response.Redirect("~/Member/Dashboard.aspx");
                }
                else
                {
                    StatusMessage.Text = "<div class='alert alert-danger'>An error occurred during registration. Please try again.</div>";
                }
            }
        }

        private bool UserExists(string username)
        {
            try
            {
                // Load Member.xml
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Member.xml");

                if (File.Exists(path))
                {
                    doc.Load(path);
                    XmlNode existingUser = doc.SelectSingleNode($"//user[username='{username}']");
                    return existingUser != null;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }

        private bool AddUser(string username, string password, string email)
        {
            try
            {
                // Hash the password
                string hashedPassword = SecurityHelper.HashPassword(password);

                // Load or create Member.xml
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Member.xml");

                if (File.Exists(path))
                {
                    doc.Load(path);
                }
                else
                {
                    // Create new XML document structure
                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                    doc.AppendChild(xmlDeclaration);
                    XmlElement rootElement = doc.CreateElement("users");
                    doc.AppendChild(rootElement);
                }

                // Get the root element
                XmlElement rootNode = doc.DocumentElement;

                // Create a new user node
                XmlElement userNode = doc.CreateElement("user");

                // Add username element
                XmlElement usernameElement = doc.CreateElement("username");
                usernameElement.InnerText = username;
                userNode.AppendChild(usernameElement);

                // Add password element
                XmlElement passwordElement = doc.CreateElement("password");
                passwordElement.InnerText = hashedPassword;
                userNode.AppendChild(passwordElement);

                // Add email element
                XmlElement emailElement = doc.CreateElement("email");
                emailElement.InnerText = email;
                userNode.AppendChild(emailElement);

                // Add registration date element
                XmlElement registrationDateElement = doc.CreateElement("registrationDate");
                registrationDateElement.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                userNode.AppendChild(registrationDateElement);

                // Add the user node to the root
                rootNode.AppendChild(userNode);

                // Save the XML document
                doc.Save(path);

                return true;
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }
    }
}