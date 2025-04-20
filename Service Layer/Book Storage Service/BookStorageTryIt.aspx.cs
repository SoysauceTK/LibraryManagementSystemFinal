using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryManagementSystem.Service_Layer.Book_Storage_Service;

namespace LibraryManagementSystem.Service_Layer.Book_Storage_Service
{
    public partial class BookStorageTryIt : Page
    {
        private BookStorage bookStorage;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Disable any authentication redirects for this page
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            bookStorage = new BookStorage();
        }

        // Get Operations
        protected void btnGetAllBooks_Click(object sender, EventArgs e)
        {
            var allBooks = bookStorage.GetAllBooks();
            DisplayResults(allBooks);
        }

        protected void btnGetBookById_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBookId.Text))
            {
                var book = bookStorage.GetBookById(txtBookId.Text);
                if (book != null)
                {
                    DisplayResults(new List<Book> { book });
                }
                else
                {
                    ShowMessage("Book not found.", "warning");
                }
            }
            else
            {
                ShowMessage("Please enter a book ID.", "danger");
            }
        }

        protected void btnGetBooksByCategory_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCategory.Text))
            {
                var books = bookStorage.GetBooksByCategory(txtCategory.Text);
                DisplayResults(books);
            }
            else
            {
                ShowMessage("Please enter a category.", "danger");
            }
        }

        // Add Book Operations
        protected void btnShowAddForm_Click(object sender, EventArgs e)
        {
            pnlAddBook.Visible = true;
            btnShowAddForm.Visible = false;
        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {
            ClearAddForm();
            pnlAddBook.Visible = false;
            btnShowAddForm.Visible = true;
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                var newBook = new Book
                {
                    Title = txtTitle.Text,
                    Author = txtAuthor.Text,
                    ISBN = txtISBN.Text,
                    Category = txtCategory.Text,
                    PublicationYear = int.Parse(txtYear.Text),
                    Publisher = txtPublisher.Text,
                    CopiesAvailable = int.Parse(txtCopies.Text),
                    Description = txtDescription.Text
                };

                var addedBook = bookStorage.AddBook(newBook);
                ClearAddForm();
                pnlAddBook.Visible = false;
                btnShowAddForm.Visible = true;
                DisplayResults(new List<Book> { addedBook });
                ShowMessage("Book added successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error adding book: {ex.Message}", "danger");
            }
        }

        // Update Book Operations
        protected void btnLoadForUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUpdateId.Text))
            {
                var book = bookStorage.GetBookById(txtUpdateId.Text);
                if (book != null)
                {
                    hdnUpdateId.Value = book.Id;
                    txtUpdateTitle.Text = book.Title;
                    txtUpdateAuthor.Text = book.Author;
                    txtUpdateISBN.Text = book.ISBN;
                    txtUpdateCategory.Text = book.Category;
                    txtUpdateYear.Text = book.PublicationYear.ToString();
                    txtUpdatePublisher.Text = book.Publisher;
                    txtUpdateCopies.Text = book.CopiesAvailable.ToString();
                    txtUpdateDescription.Text = book.Description;
                    pnlUpdateBook.Visible = true;
                }
                else
                {
                    ShowMessage("Book not found.", "warning");
                }
            }
            else
            {
                ShowMessage("Please enter a book ID.", "danger");
            }
        }

        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            ClearUpdateForm();
            pnlUpdateBook.Visible = false;
        }

        protected void btnUpdateBook_Click(object sender, EventArgs e)
        {
            try
            {
                var updatedBook = new Book
                {
                    Id = hdnUpdateId.Value,
                    Title = txtUpdateTitle.Text,
                    Author = txtUpdateAuthor.Text,
                    ISBN = txtUpdateISBN.Text,
                    Category = txtUpdateCategory.Text,
                    PublicationYear = int.Parse(txtUpdateYear.Text),
                    Publisher = txtUpdatePublisher.Text,
                    CopiesAvailable = int.Parse(txtUpdateCopies.Text),
                    Description = txtUpdateDescription.Text
                };

                var result = bookStorage.UpdateBook(updatedBook);
                ClearUpdateForm();
                pnlUpdateBook.Visible = false;
                DisplayResults(new List<Book> { result });
                ShowMessage("Book updated successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error updating book: {ex.Message}", "danger");
            }
        }

        // Delete Book Operation
        protected void btnDeleteBook_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDeleteId.Text))
            {
                bool success = bookStorage.DeleteBook(txtDeleteId.Text);
                if (success)
                {
                    ShowMessage("Book deleted successfully!", "success");
                    txtDeleteId.Text = string.Empty;
                }
                else
                {
                    ShowMessage("Failed to delete book.", "danger");
                }
            }
            else
            {
                ShowMessage("Please enter a book ID.", "danger");
            }
        }

        // Helper Methods
        private void DisplayResults(List<Book> books)
        {
            if (books == null || books.Count == 0)
            {
                ShowMessage("No results found.", "info");
                return;
            }

            litResult.Text = "<table class='table table-striped table-bordered'><thead class='thead-dark'><tr>" +
                "<th>ID</th><th>Title</th><th>Author</th><th>Category</th><th>Year</th><th>Available</th>" +
                "</tr></thead><tbody>" +
                string.Join("", books.ConvertAll(b =>
                    $"<tr>" +
                    $"<td>{b.Id}</td>" +
                    $"<td>{b.Title}</td>" +
                    $"<td>{b.Author}</td>" +
                    $"<td>{b.Category}</td>" +
                    $"<td>{b.PublicationYear}</td>" +
                    $"<td>{b.CopiesAvailable}</td>" +
                    $"</tr>")) +
                "</tbody></table>";
        }

        private void ShowMessage(string message, string type)
        {
            litResult.Text = $"<div class='alert alert-{type}'>{message}</div>";
        }

        private void ClearAddForm()
        {
            txtTitle.Text = string.Empty;
            txtAuthor.Text = string.Empty;
            txtISBN.Text = string.Empty;
            txtCategory.Text = string.Empty;
            txtYear.Text = string.Empty;
            txtPublisher.Text = string.Empty;
            txtCopies.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }

        private void ClearUpdateForm()
        {
            hdnUpdateId.Value = string.Empty;
            txtUpdateId.Text = string.Empty;
            txtUpdateTitle.Text = string.Empty;
            txtUpdateAuthor.Text = string.Empty;
            txtUpdateISBN.Text = string.Empty;
            txtUpdateCategory.Text = string.Empty;
            txtUpdateYear.Text = string.Empty;
            txtUpdatePublisher.Text = string.Empty;
            txtUpdateCopies.Text = string.Empty;
            txtUpdateDescription.Text = string.Empty;
        }
    }
}