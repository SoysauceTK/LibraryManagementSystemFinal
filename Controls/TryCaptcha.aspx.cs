using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryManagementSystem.Controls
{
    public partial class TryCaptcha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize the captcha on first load
                captchaControl.GenerateNewCaptcha();
            }
        }

        protected void btnVerifyCaptcha_Click(object sender, EventArgs e)
        {
            string userInput = txtCaptchaInput.Text.Trim();
            
            if (string.IsNullOrEmpty(userInput))
            {
                ShowResult("Please enter the CAPTCHA text", false);
                return;
            }

            bool isValid = captchaControl.Validate(userInput);
            
            if (isValid)
            {
                ShowResult("CAPTCHA verification successful!", true);
                // Generate a new captcha after successful verification
                captchaControl.GenerateNewCaptcha();
                txtCaptchaInput.Text = string.Empty;
            }
            else
            {
                ShowResult("CAPTCHA verification failed. Please try again.", false);
                // Generate a new captcha after failed verification
                captchaControl.GenerateNewCaptcha();
                txtCaptchaInput.Text = string.Empty;
            }
        }

        protected void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            // Refresh the captcha
            captchaControl.GenerateNewCaptcha();
            txtCaptchaInput.Text = string.Empty;
            pnlResults.Visible = false;
        }

        private void ShowResult(string message, bool success)
        {
            pnlResults.Visible = true;
            pnlResults.CssClass = success ? "alert alert-success" : "alert alert-danger";
            litResults.Text = message;
        }
    }
}