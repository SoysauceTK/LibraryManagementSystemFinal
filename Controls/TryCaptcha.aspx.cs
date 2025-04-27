using System;
using System.Web.UI;

namespace LibraryManagementSystem.Controls
{
    public partial class TryCaptcha : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any previous result
                ResultLiteral.Text = string.Empty;
            }
        }

        protected void VerifyButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (testCaptcha.Validate())
                {
                    ResultLiteral.Text = "<div class='alert alert-success'>CAPTCHA verification successful!</div>";
                }
                else
                {
                    ResultLiteral.Text = "<div class='alert alert-danger'>CAPTCHA verification failed. Please try again.</div>";
                    testCaptcha.GenerateCaptcha(); // Generate a new CAPTCHA
                }

                // Clear the input
                CaptchaInput.Text = string.Empty;
            }
        }
    }
}