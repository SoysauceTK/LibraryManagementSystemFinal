using System;
using System.Web;
using System.Web.UI;
using Microsoft.Owin;

namespace LibraryManagementSystem
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Unnamed_LoggingOut(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
        }
    }
}
