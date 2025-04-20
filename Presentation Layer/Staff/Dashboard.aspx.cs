using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace LibraryManagementSystem.Presentation_Layer.Staff
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("~/Presentation Layer/Staff/Login.aspx?ReturnUrl=" +
                        Server.UrlEncode(Request.Url.PathAndQuery));
                    return;
                }

                if (!User.IsInRole("Staff"))
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Presentation Layer/Public/Default.aspx");
                    return;
                }
            }
        }
    }
}