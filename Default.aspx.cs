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

        // Member Dashboard access
        protected void lnkMemberDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Member/Dashboard.aspx");
        }

        protected void lnkMemberDashboardStaffLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Member/Login.aspx");
        }

        protected void lnkMemberLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Member/Login.aspx");
        }

        // Member Registration access
        protected void lnkMemberRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Member/Register.aspx");
        }

        protected void lnkMemberRegisterLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Member/Register.aspx");
        }

        // Staff Dashboard access
        protected void lnkStaffDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Staff/Dashboard.aspx");
        }

        protected void lnkStaffDashboardMemberLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Staff/Login.aspx");
        }

        protected void lnkStaffLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Staff/Login.aspx");
        }

        // Service Try It pages
        protected void lnkTryBookStorage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Services/TryBookStorage.aspx");
        }

        protected void lnkTryBookSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Services/TryBookSearch.aspx");
        }

        protected void lnkTryCaptcha_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Controls/TryCaptcha.aspx");
        }

        protected void lnkTryCookies_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Controls/TryCookies.aspx");
        }
    }
}