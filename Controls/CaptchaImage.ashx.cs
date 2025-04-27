// CaptchaImage.ashx.cs
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;

namespace LibraryManagementSystem.Controls
{
    public class CaptchaImage : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string captchaText = context.Session["CaptchaText"] as string;
            if (string.IsNullOrEmpty(captchaText))
            {
                captchaText = "ERROR";
            }

            using (Bitmap bitmap = new Bitmap(200, 50))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Fill background
                graphics.Clear(Color.White);

                // Add noise
                Random random = new Random();
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(bitmap.Width);
                    int y = random.Next(bitmap.Height);
                    bitmap.SetPixel(x, y, Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                }

                // Draw text
                Font font = new Font("Arial", 25, FontStyle.Bold | FontStyle.Italic);
                Brush brush = new SolidBrush(Color.DarkBlue);
                graphics.DrawString(captchaText, font, brush, 10, 10);

                // Save to output stream
                bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            }
        }

        public bool IsReusable => false;
    }
}