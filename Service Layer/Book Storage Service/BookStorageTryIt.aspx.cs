using System;
using System.Web.UI;
using LibraryManagementSystem.Service_Layer.Book_Storage_Service;

namespace LibraryManagementSystem.Service_Layer.Book_Storage_Service
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
                var bookStorage = new BookStorage();
                var books = bookStorage.GetAllBooks();
                litResult.Text = "<ul>" + string.Join("", books.ConvertAll(b => $"<li>{b.Title}</li>")) + "</ul>";
            }
        }
    }
}
