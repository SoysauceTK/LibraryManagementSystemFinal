using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using LMS.Security;
using LibraryManagementSystem.Controls; 

namespace LibraryManagementSystem.Staff
{
    public partial class Login : Page
    {
        // Add a reference to the CaptchaControl on the page
        protected CaptchaControl captchaControl; // Adjust namespace if needed

        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect if staff is already logged in
            if (User.Identity.IsAuthenticated)
            {
                // Check if the user has Staff role
                if (User.IsInRole("Staff"))
                {
                    Response.Redirect("~/Staff/Dashboard.aspx");
                }
                else
                {
                    // User is authenticated but not as Staff, log them out first
                    FormsAuthentication.SignOut();
                    // Clear the response to avoid automatic redirects
                    Response.Clear();
                }
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            // Clear previous error messages
            FailureText.Text = "";
            CaptchaFailureText.Text = ""; // Clear captcha error too

            // 1. Validate Page Controls (Username, Password required)
            if (IsValid)
            {
                // 2. Validate CAPTCHA first
                if (captchaControl.Validate()) // Call the Validate method of the CaptchaControl
                {
                    // 3. If CAPTCHA is valid, validate credentials
                    if (ValidateStaff(Username.Text, Password.Text))
                    {
                        // Create authentication cookie with staff role
                        FormsAuthentication.SetAuthCookie(Username.Text, false);

                        // Add staff role for this session
                        HttpCookie authCookie = FormsAuthentication.GetAuthCookie(Username.Text, false);
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                        FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                            ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                            ticket.IsPersistent, "Staff", ticket.CookiePath);
                        authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                        Response.Cookies.Add(authCookie);

                        // Get return URL if any
                        string returnUrl = Request.QueryString["ReturnUrl"];
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            Response.Redirect(returnUrl);
                        }
                        else
                        {
                            Response.Redirect("~/Staff/Dashboard.aspx");
                        }
                    }
                    else
                    {
                        FailureText.Text = "<div class='alert alert-danger'>Invalid username or password.</div>";
                        // Regenerate captcha on failed login attempt as well
                        captchaControl.GenerateCaptcha();
                    }
                }
                else
                {
                    // CAPTCHA validation failed
                    CaptchaFailureText.Text = "<div class='text-danger'>CAPTCHA validation failed. Please try again.</div>";
                    // The captcha control's Validate method already refreshes the captcha on failure
                }
            }
            else
            {
                // IsValid is false, meaning a RequiredFieldValidator failed.
                // ASP.NET validation controls will display their messages.
                // We might still want to regenerate the captcha here if desired,
                // otherwise the user has to re-type it after fixing the required field.
                captchaControl.GenerateCaptcha();
            }
        }

        private bool ValidateStaff(string username, string password)
        {
            try
            {
                // Load Staff.xml
                XmlDocument doc = new XmlDocument();
                string path = Server.MapPath("~/App_Data/Staff.xml");
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
                System.IO.File.AppendAllText(logPath, "Error in Staff Login: " + DateTime.Now.ToString() + "\n");
                System.IO.File.AppendAllText(logPath, ex.ToString() + "\n");

                FailureText.Text = "<div class='alert alert-danger'>An error occurred during login. Please try again.</div>";
                return false;
            }
        }
    }
}