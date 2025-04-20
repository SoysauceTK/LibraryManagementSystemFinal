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

        protected void NavigateToSearchTryIt(object sender, EventArgs e)
        {
            Response.Redirect("~/Service_Layer/Book_Search_Service/SearchTryIt.aspx");
        }

        protected void NavigateToBookStorageTryIt(object sender, EventArgs e)
        {
            Response.Redirect("~/Service_Layer/Book_Storage_Service/BookStorageTryIt.aspx");
        }
    }
}