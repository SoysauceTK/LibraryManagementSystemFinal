using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace LibraryManagementSystem.Member
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Presentation Layer/Member/Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Presentation Layer/Public/Default.aspx");
        }
    }
}