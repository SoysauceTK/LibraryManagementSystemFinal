using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;

namespace LibraryManagementSystem
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the page refreshes authentication state
            if (Request.IsAuthenticated)
            {
                // You can add any role-specific logic here if needed
            }
        }

        protected void Unnamed_LoggingOut(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/");
        }
    }
}