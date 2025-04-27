using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Threading.Tasks;
using System.ServiceModel;
using LMS.BookStorage.Models; // Using the original Book model directly

namespace LibraryManagementSystem.Staff
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated and in Staff role
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Staff"))
            {
                Response.Redirect("~/Staff/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                // Display username
                UserNameLiteral.Text = User.Identity.Name;

                // Load quick stats asynchronously
                RegisterAsyncTask(new PageAsyncTask(LoadQuickStats));

                // Load activity log (sample data)
                LoadActivityLog();
            }
        }

        private async Task LoadQuickStats()
        {
            try
            {
                // Create BookService client
                using (var bookClient = new BookServiceReference.BookServiceClient())
                {
                    // Call GetAllBooks method
                    var books = await bookClient.GetAllBooksAsync();

                    // Update the UI with the book count
                    TotalBooksLiteral.Text = books?.Length.ToString("N0") ?? "0";
                }
            }
            catch (Exception ex)
            {
                TotalBooksLiteral.Text = "Error";
                System.Diagnostics.Debug.WriteLine($"Error loading total books: {ex.Message}");
            }

            // Placeholder data for other stats
            BorrowedBooksLiteral.Text = "278 (Sample)";
            MembersLiteral.Text = "356 (Sample)";
            OverdueLiteral.Text = "12 (Sample)";
        }

        private void LoadActivityLog()
        {
            if (ActivityLogGridView.DataSource == null)
            {
                List<ActivityLogEntry> activityLog = new List<ActivityLogEntry>
                 {
                     new ActivityLogEntry { Date = DateTime.Now.AddHours(-3), Action = "Book Borrowed", Details = "To Kill a Mockingbird by Harper Lee", User = "john_doe" },
                     new ActivityLogEntry { Date = DateTime.Now.AddHours(-5), Action = "New Member", Details = "sarah_smith joined the library", User = "system" },
                     new ActivityLogEntry { Date = DateTime.Now.AddDays(-1), Action = "Book Returned", Details = "1984 by George Orwell", User = "mike_jones" }
                 };
                ActivityLogGridView.DataSource = activityLog;
            }

            ActivityLogGridView.DataBind();
        }

        protected async void AddBookButton_Click(object sender, EventArgs e)
        {
            BookAddStatusPanel.Visible = true;

            if (Page.IsValid)
            {
                try
                {
                    // Create new Book object using the LMS.BookStorage.Models.Book class
                    var book = new Book
                    {
                        Title = BookTitle.Text,
                        Author = BookAuthor.Text,
                        ISBN = BookISBN.Text,
                        Category = BookCategory.SelectedValue,
                        Description = BookDescription.Text,
                        CopiesAvailable = int.Parse(BookCopies.Text),
                        PublicationYear = int.Parse(BookPublicationYear.Text),
                        Publisher = "Unknown"
                    };

                    // Create BookService client
                    using (var bookClient = new BookServiceReference.BookServiceClient())
                    {
                        // Call AddBook method
                        var addedBook = await bookClient.AddBookAsync(book);

                        if (addedBook != null)
                        {
                            BookAddStatus.Text = "<div class='alert alert-success'>Book added successfully!</div>";

                            // Log the activity
                            LogBookAddition(book.Title, book.Author);

                            // Clear the form
                            ClearBookForm();

                            // Refresh stats
                            await LoadQuickStats();
                        }
                        else
                        {
                            BookAddStatus.Text = "<div class='alert alert-danger'>Error adding book: Service returned null.</div>";
                        }
                    }
                }
                catch (FaultException fex)
                {
                    BookAddStatus.Text = $"<div class='alert alert-danger'>Service error: {fex.Message}</div>";
                }
                catch (CommunicationException cex)
                {
                    BookAddStatus.Text = $"<div class='alert alert-warning'>Could not connect to Book Service. Please try again later. ({cex.Message})</div>";
                }
                catch (Exception ex)
                {
                    BookAddStatus.Text = $"<div class='alert alert-danger'>An unexpected error occurred: {ex.Message}</div>";
                }
            }
        }

        private void ClearBookForm()
        {
            BookTitle.Text = string.Empty;
            BookAuthor.Text = string.Empty;
            BookISBN.Text = string.Empty;
            BookCategory.SelectedIndex = 0;
            BookPublicationYear.Text = string.Empty;
            BookCopies.Text = string.Empty;
            BookDescription.Text = string.Empty;
        }

        private void LogBookAddition(string title, string author)
        {
            var activityLog = ActivityLogGridView.DataSource as List<ActivityLogEntry>;
            if (activityLog == null)
            {
                activityLog = new List<ActivityLogEntry>();
            }

            activityLog.Insert(0, new ActivityLogEntry
            {
                Date = DateTime.Now,
                Action = "Book Added",
                Details = $"{title} by {author}",
                User = User.Identity.Name ?? "Unknown"
            });

            ActivityLogGridView.DataSource = activityLog;
            ActivityLogGridView.DataBind();
        }

        protected async void SearchButton_Click(object sender, EventArgs e)
        {
            string query = SearchTextBox.Text.Trim();
            SearchStatusPanel.Visible = true;

            if (string.IsNullOrWhiteSpace(query))
            {
                SearchStatusLiteral.Text = "<div class='alert alert-warning'>Please enter a search term.</div>";
                SearchResultsGridView.DataSource = null;
                SearchResultsGridView.DataBind();
                return;
            }

            SearchStatusLiteral.Text = "<div class='alert alert-info'>Searching...</div>";

            try
            {
                // Use BookService for searching by retrieving all books
                using (var bookClient = new BookServiceReference.BookServiceClient())
                {
                    var allBooks = await bookClient.GetAllBooksAsync();

                    // Perform basic search filtering
                    var results = new List<Book>();

                    if (allBooks != null)
                    {
                        query = query.ToLower();

                        foreach (var book in allBooks)
                        {
                            if ((book.Title != null && book.Title.ToLower().Contains(query)) ||
                                (book.Author != null && book.Author.ToLower().Contains(query)) ||
                                (book.ISBN != null && book.ISBN.ToLower().Contains(query)) ||
                                (book.Category != null && book.Category.ToLower().Contains(query)) ||
                                (book.Description != null && book.Description.ToLower().Contains(query)))
                            {
                                results.Add(book);
                            }
                        }
                    }

                    SearchResultsGridView.DataSource = results;
                    SearchResultsGridView.DataBind();

                    if (results.Count > 0)
                    {
                        SearchStatusLiteral.Text = $"<div class='alert alert-success'>Found {results.Count} book(s) matching '{System.Web.HttpUtility.HtmlEncode(query)}'.</div>";
                    }
                    else
                    {
                        SearchStatusLiteral.Text = $"<div class='alert alert-warning'>No books found matching '{System.Web.HttpUtility.HtmlEncode(query)}'.</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                SearchStatusLiteral.Text = $"<div class='alert alert-danger'>Error during search: {ex.Message}</div>";
                SearchResultsGridView.DataSource = null;
                SearchResultsGridView.DataBind();
            }
        }
    }

    public class ActivityLogEntry
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public string User { get; set; }
    }
}