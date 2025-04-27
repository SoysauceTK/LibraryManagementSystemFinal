using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LMS.BookStorage;
using LMS.BookStorage.Models;
using System.Text;
using Newtonsoft.Json;

namespace LibraryManagementSystem.Services
{
    public partial class TryBookStorage : System.Web.UI.Page
    {
        private BookService _bookService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _bookService = new BookService();
        }

        protected void btnGetAllBooks_Click(object sender, EventArgs e)
        {
            try
            {
                var books = _bookService.GetAllBooks();
                DisplayResults(books);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnGetBookById_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtBookId.Text.Trim();
                if (string.IsNullOrEmpty(bookId))
                {
                    litResults.Text = "Please enter a book ID";
                    return;
                }

                var book = _bookService.GetBookById(bookId);
                DisplayResults(book);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                Book book = new Book
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

                var result = _bookService.AddBook(book);
                DisplayResults(result);
                
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

        protected void btnGetBooksByCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string category = txtSearchCategory.Text.Trim();
                if (string.IsNullOrEmpty(category))
                {
                    litResults.Text = "Please enter a category";
                    return;
                }

                var books = _bookService.GetBooksByCategory(category);
                DisplayResults(books);
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnUpdateInventory_Click(object sender, EventArgs e)
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

                InventoryUpdate update = new InventoryUpdate
                {
                    BookId = bookId,
                    QuantityChange = quantityChange
                };

                bool result = _bookService.UpdateInventory(update);
                litResults.Text = result ? 
                    "Inventory updated successfully" : 
                    "Failed to update inventory. Book might not exist.";
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
        }

        protected void btnDeleteBook_Click(object sender, EventArgs e)
        {
            try
            {
                string bookId = txtDeleteBookId.Text.Trim();
                if (string.IsNullOrEmpty(bookId))
                {
                    litResults.Text = "Please enter a book ID";
                    return;
                }

                bool result = _bookService.DeleteBook(bookId);
                litResults.Text = result ? 
                    "Book deleted successfully" : 
                    "Failed to delete book. Book might not exist.";
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