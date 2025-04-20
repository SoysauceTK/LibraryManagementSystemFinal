using System;
using System.Web.UI;
using System.Xml;
using System.IO;
using LMS.Security;

namespace LibraryManagementSystem.Presentation_Layer.Member
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No authentication checks here - this page should be accessible to all
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    string xmlPath = Server.MapPath("~/App_Data/Member.xml");
                    XmlDocument doc = new XmlDocument();

                    // Load or create the XML file
                    if (File.Exists(xmlPath))
                    {
                        doc.Load(xmlPath);
                    }
                    else
                    {
                        doc.LoadXml("<users></users>");
                    }

                    // Check if username already exists
                    if (doc.SelectSingleNode($"//user[username='{Username.Text}']") != null)
                    {
                        litMessage.Text = "Username already exists. Please choose another.";
                        return;
                    }

                    // Create new user node
                    XmlElement newUser = doc.CreateElement("user");
                    newUser.AppendChild(CreateNode(doc, "username", Username.Text));
                    newUser.AppendChild(CreateNode(doc, "password", SecurityHelper.HashPassword(Password.Text)));
                    newUser.AppendChild(CreateNode(doc, "email", Email.Text));

                    doc.DocumentElement.AppendChild(newUser);
                    doc.Save(xmlPath);

                    // Redirect to login after successful registration
                    Response.Redirect("~/Presentation Layer/Member/Login.aspx?registered=true");
                }
                catch (Exception ex)
                {
                    litMessage.Text = "Registration failed. Please try again later.";
                    // Log the error
                }
            }
        }

        private XmlNode CreateNode(XmlDocument doc, string name, string value)
        {
            XmlNode node = doc.CreateElement(name);
            node.InnerText = value;
            return node;
        }
    }
}