using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using System.ServiceModel;

namespace LibraryManagementSystem.Services
{
    public partial class TryBookStorage : System.Web.UI.Page
    {
        private BookServiceReference.BookServiceClient CreateBookServiceClient()
        {
            var client = new BookServiceReference.BookServiceClient("BasicHttpBinding_IBookService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            return client;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnGetAllBooks_Click(object sender, EventArgs e)
        {
            try
            {
                using (var bookClient = CreateBookServiceClient())
                {
                    var books = await bookClient.GetAllBooksAsync();
                    DisplayResults(books);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected async void btnGetBookById_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtBookId.Text.Trim();
                if (string.IsNullOrEmpty(bookId))
                {
                    litResults.Text = "Please enter a book ID";
                    return;
                }

                using (var bookClient = CreateBookServiceClient())
                {
                    var book = await bookClient.GetBookByIdAsync(bookId);
                    DisplayResults(book);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected async void btnAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                var book = new BookServiceReference.Book
                {
                    Title = txtTitle.Text.Trim(),
                    Author = txtAuthor.Text.Trim(),
                    ISBN = txtISBN.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    CopiesAvailable = Convert.ToInt32(txtCopiesAvailable.Text)
                };

                int year;
                if (int.TryParse(txtPublicationYear.Text, out year))
                {
                    book.PublicationYear = year;
                }

                using (var bookClient = CreateBookServiceClient())
                {
                    var result = await bookClient.AddBookAsync(book);
                    DisplayResults(result);
                }
                
                // Clear fields for next book
                txtTitle.Text = string.Empty;
                txtAuthor.Text = string.Empty;
                txtISBN.Text = string.Empty;
                txtCategory.Text = string.Empty;
                txtPublicationYear.Text = string.Empty;
                txtCopiesAvailable.Text = string.Empty;
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected async void btnGetBooksByCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string category = txtSearchCategory.Text.Trim();
                if (string.IsNullOrEmpty(category))
                {
                    litResults.Text = "Please enter a category";
                    return;
                }

                using (var bookClient = CreateBookServiceClient())
                {
                    var books = await bookClient.GetBooksByCategoryAsync(category);
                    DisplayResults(books);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected async void btnUpdateInventory_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtUpdateBookId.Text.Trim();
                if (string.IsNullOrEmpty(bookId))
                {
                    litResults.Text = "Please enter a book ID";
                    return;
                }

                int quantityChange;
                if (!int.TryParse(txtQuantityChange.Text, out quantityChange))
                {
                    litResults.Text = "Please enter a valid quantity change";
                    return;
                }

                var update = new BookServiceReference.InventoryUpdate
                {
                    BookId = bookId,
                    QuantityChange = quantityChange
                };

                using (var bookClient = CreateBookServiceClient())
                {
                    bool result = await bookClient.UpdateInventoryAsync(update);
                    litResults.Text = result ? 
                        "Inventory updated successfully" : 
                        "Failed to update inventory. Book might not exist.";
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected async void btnDeleteBook_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtDeleteBookId.Text.Trim();
                if (string.IsNullOrEmpty(bookId))
                {
                    litResults.Text = "Please enter a book ID";
                    return;
                }

                using (var bookClient = CreateBookServiceClient())
                {
                    bool result = await bookClient.DeleteBookAsync(bookId);
                    litResults.Text = result ? 
                        "Book deleted successfully" : 
                        "Failed to delete book. Book might not exist.";
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        private void DisplayResults<T>(T data)
        {
            if (data == null)
            {
                litResults.Text = "No results found";
                return;
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            litResults.Text = HttpUtility.HtmlEncode(json);
        }

        private void DisplayError(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Error:");
            sb.AppendLine(ex.Message);
            
            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception:");
                sb.AppendLine(ex.InnerException.Message);
            }
            
            litResults.Text = HttpUtility.HtmlEncode(sb.ToString());
        }
    }
}