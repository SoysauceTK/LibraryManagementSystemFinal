using System;
using System.Web.UI;

namespace LibraryManagementSystem
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnMember_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Member/Login.aspx");
        }

        protected void btnStaff_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Staff/Login.aspx");
        }
    }
}