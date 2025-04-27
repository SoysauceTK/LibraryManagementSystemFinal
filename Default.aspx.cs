using System;
using System.Web.UI;

namespace LibraryManagementSystem
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Make the components table visible
            pnlLoggedInContent.Visible = true;
        }

        protected void btnMember_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Member/Login.aspx");
        }

        protected void btnStaff_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Staff/Login.aspx");
        }

        protected void btnMemberRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Member/Register.aspx");
        }

        protected void btnTryBookStorage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Services/TryBookStorage.aspx");
        }

        protected void btnTryBookSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Services/TryBookSearch.aspx");
        }

        protected void btnTryCaptcha_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Controls/TryCaptcha.aspx");
        }
    }
}