// CaptchaControl.ascx.cs
// DEVELOPER: Sawyer Kesti - CAPTCHA User Control - [Date]
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;

namespace LibraryManagementSystem.Controls
{
    public partial class CaptchaControl : UserControl
    {
        // The CAPTCHA text to verify
        private string _captchaText;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateCaptcha();
            }
        }

        // Generates a new CAPTCHA image and text
        public void GenerateCaptcha()
        {
            _captchaText = GenerateRandomCode();
            Session["CaptchaText"] = _captchaText;

            imgCaptcha.ImageUrl = "~/Controls/CaptchaImage.ashx?t=" + DateTime.Now.Ticks;
        }

        // Generates a random code for the CAPTCHA
        private string GenerateRandomCode()
        {
            Random random = new Random();
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            string code = "";

            for (int i = 0; i < 6; i++)
            {
                code += chars[random.Next(chars.Length)];
            }

            return code;
        }

        // Validates the CAPTCHA input
        public bool Validate(string input)
        {
            string captchaText = Session["CaptchaText"] as string;
            return !string.IsNullOrEmpty(captchaText) &&
                   captchaText.Equals(input, StringComparison.OrdinalIgnoreCase);
        }

        // Refreshes the CAPTCHA image
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }
    }
}