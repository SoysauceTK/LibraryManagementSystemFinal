using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Threading.Tasks;
using System.ServiceModel;
using LMS.BookStorage.Models;
using System.Runtime.Serialization;

namespace LibraryManagementSystem.Staff
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Staff"))
            {
                Response.Redirect("~/Staff/Login.aspx", true);
                return;
            }

            if (!IsPostBack)
            {
                UserNameLiteral.Text = User.Identity.Name;
                RegisterAsyncTask(new PageAsyncTask(LoadQuickStats));
                LoadActivityLog();
            }
        }

        private BookServiceReference.BookServiceClient CreateBookServiceClient()
        {
            var client = new BookServiceReference.BookServiceClient("BasicHttpBinding_IBookService");
            client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(30);
            client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(30);
            return client;
        }

        private async Task LoadQuickStats()
        {
            try
            {
                using (var bookClient = CreateBookServiceClient())
                {
                    var books = await bookClient.GetAllBooksAsync();
                    TotalBooksLiteral.Text = books?.Length.ToString("N0") ?? "0";
                }
            }
            catch (Exception ex)
            {
                HandleServiceError(ex, "load quick stats");
                TotalBooksLiteral.Text = "Error";
            }

            // Placeholder data for other stats
            BorrowedBooksLiteral.Text = "278";
            MembersLiteral.Text = "356";
            OverdueLiteral.Text = "12";
        }

        private void LoadActivityLog()
        {
            var activityLog = new List<ActivityLogEntry>
            {
                new ActivityLogEntry { Date = DateTime.Now.AddHours(-3), Action = "Book Borrowed", Details = "To Kill a Mockingbird by Harper Lee", User = "john_doe" },
                new ActivityLogEntry { Date = DateTime.Now.AddHours(-5), Action = "New Member", Details = "sarah_smith joined the library", User = "system" },
                new ActivityLogEntry { Date = DateTime.Now.AddDays(-1), Action = "Book Returned", Details = "1984 by George Orwell", User = "mike_jones" }
            };

            ActivityLogGridView.DataSource = activityLog;
            ActivityLogGridView.DataBind();
        }

        protected async void AddBookButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            BookAddStatusPanel.Visible = true;

            try
            {
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

                using (var bookClient = CreateBookServiceClient())
                {
                    var addedBook = await bookClient.AddBookAsync(book);

                    if (addedBook != null)
                    {
                        BookAddStatus.Text = "<div class='alert alert-success'>Book added successfully!</div>";
                        LogBookAddition(book.Title, book.Author);
                        ClearBookForm();
                        await LoadQuickStats();
                    }
                    else
                    {
                        BookAddStatus.Text = "<div class='alert alert-danger'>Error adding book</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                HandleServiceError(ex, "add book");
                BookAddStatus.Text = $"<div class='alert alert-danger'>{GetUserFriendlyError(ex)}</div>";
            }
        }

        private void HandleServiceError(Exception ex, string operation)
        {
            System.Diagnostics.Debug.WriteLine($"Error during {operation}: {ex}");

            // 👇 ADD this for now to see the real error on screen
            Response.Write($"<pre>Real error during {operation}: {ex}</pre>");

            if (ex is CommunicationException || ex is TimeoutException)
            {
                LogErrorToDatabase($"Service communication error during {operation}", ex);
            }
        }


        private string GetUserFriendlyError(Exception ex)
        {
            string friendlyMessage;

            if (ex is CommunicationException)
                friendlyMessage = "Service is unavailable. Please try again later.";
            else if (ex is TimeoutException)
                friendlyMessage = "Request timed out. Please check your connection.";
            else if (ex is FaultException)
                friendlyMessage = "The server reported an error while processing your request.";
            else if (ex is ProtocolException)
                friendlyMessage = "There was a communication protocol issue. Please contact support.";
            else if (ex is EndpointNotFoundException)
                friendlyMessage = "Could not connect to the service. Please try again later.";
            else if (ex is ServerTooBusyException)
                friendlyMessage = "The server is currently busy. Please try again later.";
            else if (ex is SerializationException)
                friendlyMessage = "Data format mismatch. Please contact support.";
            else
                friendlyMessage = "An unexpected error occurred. Please contact support.";

#if DEBUG
            // 👇 Add detailed technical error in DEBUG mode
            friendlyMessage += $"<br/><strong>Technical details:</strong> {ex.Message}";
#endif

            return friendlyMessage;
        }


        private void LogErrorToDatabase(string message, Exception ex)
        {
            // Implement your error logging here
            // Could use database, file system, or application insights
        }

        private void ClearBookForm()
        {
            BookTitle.Text = string.Empty;
            BookAuthor.Text = string.Empty;
            BookISBN.Text = string.Empty;
            BookCategory.SelectedIndex = 0;
            BookDescription.Text = string.Empty;
            BookCopies.Text = "1";
            BookPublicationYear.Text = DateTime.Now.Year.ToString();
        }

        private void LogBookAddition(string title, string author)
        {
            var newEntry = new ActivityLogEntry
            {
                Date = DateTime.Now,
                Action = "Book Added",
                Details = $"{title} by {author}",
                User = User.Identity.Name
            };

            var currentLog = ActivityLogGridView.DataSource as List<ActivityLogEntry> ?? new List<ActivityLogEntry>();
            currentLog.Insert(0, newEntry);

            ActivityLogGridView.DataSource = currentLog;
            ActivityLogGridView.DataBind();
        }

        protected async void SearchButton_Click(object sender, EventArgs e)
        {
            string query = SearchTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(query))
            {
                SearchStatusLiteral.Text = "<div class='alert alert-warning'>Please enter a search term</div>";
                SearchResultsGridView.DataSource = null;
                SearchResultsGridView.DataBind();
                return;
            }

            try
            {
                using (var bookClient = CreateBookServiceClient())
                {
                    var allBooks = await bookClient.GetAllBooksAsync();
                    var results = FilterBooks(allBooks, query);

                    SearchResultsGridView.DataSource = results;
                    SearchResultsGridView.DataBind();

                    SearchStatusLiteral.Text = results.Count > 0
                        ? $"<div class='alert alert-success'>Found {results.Count} book(s)</div>"
                        : $"<div class='alert alert-warning'>No books found</div>";
                }
            }
            catch (Exception ex)
            {
                HandleServiceError(ex, "search books");
                SearchStatusLiteral.Text = "<div class='alert alert-danger'>Search failed</div>";
            }
        }

        private List<Book> FilterBooks(Book[] allBooks, string query)
        {
            var results = new List<Book>();
            if (allBooks == null) return results;

            query = query.ToLower();
            foreach (var book in allBooks)
            {
                if (book.Title?.ToLower().Contains(query) == true ||
                    book.Author?.ToLower().Contains(query) == true ||
                    book.ISBN?.ToLower().Contains(query) == true ||
                    book.Category?.ToLower().Contains(query) == true)
                {
                    results.Add(book);
                }
            }
            return results;
        }

        public class ActivityLogEntry
        {
            public DateTime Date { get; set; }
            public string Action { get; set; }
            public string Details { get; set; }
            public string User { get; set; }
        }
    }
}