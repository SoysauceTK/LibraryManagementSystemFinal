using System;
using System.Web.UI;
using LibraryManagementSystem.Service_Layer;

namespace LibraryManagementSystem.Services
{
    public partial class BookStorageTryIt : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ddlService_SelectedIndexChanged(object sender, EventArgs e)
        {
            litResult.Text = "";
        }

        protected void btnCallService_Click(object sender, EventArgs e)
        {
            if (ddlService.SelectedValue == "GetAllBooks")
            {
                var books = new BookStorage().GetAllBooks(); // No need to fully qualify LMS.BookStorage.BookService
                litResult.Text = "<ul>" + string.Join("", books.ConvertAll(b => $"<li>{b.Title}</li>")) + "</ul>";
            }
        }
    }
}
