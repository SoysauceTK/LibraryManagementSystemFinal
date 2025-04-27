using System;
using System.Web;
using System.Web.Http;

namespace LMS.BookStorage
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Register Web API configuration
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}