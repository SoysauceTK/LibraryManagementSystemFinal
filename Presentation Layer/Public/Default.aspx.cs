using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace LibraryManagementSystem.Presentation_Layer.Public
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.IsAuthenticated)
                {
                    if (User.IsInRole("Staff"))
                    {
                        Response.Redirect("~/Presentation Layer/Staff/Dashboard.aspx");
                    }
                    else if (User.IsInRole("Member"))
                    {
                        Response.Redirect("~/Presentation Layer/Staff/Dashboard.aspx");
                    }
                }
                else
                {
                    pnlLoggedInContent.Visible = false;
                    btnMember.Visible = true;
                    btnStaff.Visible = true;
                }
            }
        }

        protected void btnMember_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation Layer/Member/Login.aspx");
        }

        protected void btnMemberRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation Layer/Member/Register.aspx");
        }

        protected void btnStaff_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Presentation Layer/Staff/Login.aspx");
        }
    }
}