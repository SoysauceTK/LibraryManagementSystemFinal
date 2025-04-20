using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;

namespace LibraryManagementSystem.Local_Component_Layer.Controls
{
    public partial class CaptchaControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateCaptcha();
            }
        }

        public void GenerateCaptcha()
        {
            string captchaText = GenerateRandomCode();
            Session["CaptchaText"] = captchaText;
            imgCaptcha.ImageUrl = "~/Local Component Layer/Controls/CaptchaImage.ashx?t=" + DateTime.Now.Ticks;
        }

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

        public bool Validate()
        {
            try
            {
                string userInput = txtCaptcha.Text.Trim();
                string captchaText = Session["CaptchaText"] as string;

                if (string.IsNullOrEmpty(captchaText))
                {
                    // Session might have expired, generate new CAPTCHA
                    GenerateCaptcha();
                    return false;
                }

                bool isValid = captchaText.Equals(userInput, StringComparison.OrdinalIgnoreCase);

                if (!isValid)
                {
                    GenerateCaptcha();
                }

                return isValid;
            }
            catch
            {
                GenerateCaptcha();
                return false;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }
    }
}